using System.Collections.Concurrent;
using System.Threading;

namespace ShUtilities.Threading
{
    /// <summary>
    /// An implementation of the Actor Model that may have changes applied to it via callbacks posted to it and then
    /// updates its externally visible state at once based on the one or more updates made to it via the callbacks.
    /// </summary>
    public class Actor
    {
        private const long ExecuteCallbacksIsRunningMarker = (long)int.MaxValue + 1;

        private readonly IActorOptions _options;
        private readonly ConcurrentQueue<CallbackAndState> _pendingCallbacks;
        private long _pendingCallbackCount;

        public Actor(IActorOptions options)
        {
            _options = options;
            _pendingCallbacks = new ConcurrentQueue<CallbackAndState>();
        }

        /// <summary>
        /// Posts one callback to the Actor to change something about itself, and after one or more 
        /// such callbacks have executed, update itself by overriding <see cref="SynchronizationContext.OperationCompleted"/>,
        /// </summary>
        public void Post(SendOrPostCallback callback, object state)
        {
            _pendingCallbacks.Enqueue(new CallbackAndState(callback, state));
            if (Interlocked.Increment(ref _pendingCallbackCount) == 1)
            {
                // The first one to post a callback starts the task to asynchronously execute this
                // and any other callbacks added until the task runs.
                _options.ScheduleAsyncExecution(ExecuteCallbacks);
            }
        }

        /// <summary>
        /// The approximate number of pending callbacks. This fields is regularly changed by multiple
        /// threads, so its value can only be used as an indication.
        /// </summary>
        public int PendingCount => unchecked((int)Volatile.Read(ref _pendingCallbackCount));

        private void ExecuteCallbacks() => TryExecuteCallbacks(false);

        /// <summary>
        /// Thread-safe method to try executing callbacks. Schedule this method on any thread to trigger execution
        /// of any pending callbacks and a reevaluation of the <see cref="IActorOptions.OperationStarted"/> and
        ///  <see cref="IActorOptions.OperationCompleted"/> methods.
        /// </summary>
        /// <param name="retryWhenMoreCallbacksNeedExecuting">Indicates whether to retry when another thread is already
        /// executing callbacks but finishes before this thread has backed out. If False, another task will be started to
        /// do so.</param>
        /// <returns>True, callbacks could be executed on the current thread. False, if another thread way already executing them.</returns>
        public bool TryExecuteCallbacks(bool retryWhenMoreCallbacksNeedExecuting)
        {
            // Only execute a limited number of callbacks, to give the actor a chance
            // to update its externally visible state in OperationCompleted() before 
            // more updates are made by more callbacks
            long callbacksToExecute = Interlocked.Add(ref _pendingCallbackCount, ExecuteCallbacksIsRunningMarker);
            bool result = callbacksToExecute < 2 * ExecuteCallbacksIsRunningMarker;

            if (!result)
            {
                // Another thread is already executing this method which is why the counter is 
                // greater than two times the marker. So we back out, but not without 
                // checking whether the other thread might already have completed and 
                // there are more callbacks that need executing. If so, we can either start
                // a task to do so, or try again ourselves; but only once, we don't want to be
                // caught up in a long recursion.
                if (MoreCallbacksNeedExecuting(ExecuteCallbacksIsRunningMarker))
                {
                    if (retryWhenMoreCallbacksNeedExecuting)
                    {
                        result = TryExecuteCallbacks(false);
                    }
                    else
                    {
                        _options.ScheduleAsyncExecution(ExecuteCallbacks);
                    }
                }
            }
            else
            {
                _options.OperationStarted();
                for (long i = ExecuteCallbacksIsRunningMarker; i < callbacksToExecute; i++)
                {
                    _pendingCallbacks.TryDequeue(out CallbackAndState callbackAndState);
                    callbackAndState.Execute();
                    // Don't decrement _pendingCallbackCount here, because that could cause
                    // the field to go to 0, another thread increments to 1 and then starts
                    // another task to execute while this method is still running
                }
                _options.OperationCompleted();

                // If more callbacks have been posted while we were executing pending ones,
                // the Post method never saw a pending count of 1 and thus it is up to this
                // method to start another task to execute callbacks asynchronously.
                if (MoreCallbacksNeedExecuting(callbacksToExecute))
                {
                    _options.ScheduleAsyncExecution(ExecuteCallbacks);
                }
            }
            return result;
        }

        private bool MoreCallbacksNeedExecuting(long decrement)
        {
            // If more callbacks have been posted while we were executing pending ones,
            // the Post method never saw a pending count of 1 and thus it is up to this
            // method to start another task to execute callbacks asynchronously.
            long pendingCallbacks = Interlocked.Add(ref _pendingCallbackCount, -decrement);
            bool result = pendingCallbacks > 0 && pendingCallbacks < ExecuteCallbacksIsRunningMarker;
            return result;
        }
    }
}
