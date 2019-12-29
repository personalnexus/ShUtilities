using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace ShUtilities.Threading
{
    /// <summary>
    /// Represents an actor in the Actor Pattern that may have changes applied to it via callbacks posted
    /// to it. In <see cref="SynchronizationContext.OperationCompleted"/> the actor can then update its
    /// externally visible state at once based on the one or more updates made to it via the callbacks.
    /// </summary>
    public class ActorSynchronizationContext: SynchronizationContext
    {
        private readonly TaskScheduler _taskScheduler;
        private readonly ConcurrentQueue<CallbackAndState> _pendingCallbacks;
        private volatile int _pendingCallbackCount;

        public ActorSynchronizationContext(TaskScheduler taskScheduler)
        {
            _taskScheduler = taskScheduler;
            _pendingCallbacks = new ConcurrentQueue<CallbackAndState>();
        }

        /// <summary>
        /// Posts one callback to the Actor to change something about itself, and after one or more 
        /// such callbacks have executed, update itself by overriding <see cref="SynchronizationContext.OperationCompleted"/>,
        /// </summary>
        public override void Post(SendOrPostCallback callback, object state)
        {
            _pendingCallbacks.Enqueue(new CallbackAndState(callback, state));
            if (Interlocked.Increment(ref _pendingCallbackCount) == 1)
            {
                // The first one to post a callback starts the task to asynchronously execute this
                // and any other callbacks added until the task runs.
                StartTaskToExecuteCallbacks();
            }
        }

        public override void Send(SendOrPostCallback callback, object state)
        {
            SynchronizationContextExtensions.SendHelper(this, callback, state);
        }

        private void StartTaskToExecuteCallbacks()
        {
            new Task(ExecuteCallbacks).Start(_taskScheduler);
        }

        private void ExecuteCallbacks()
        {
            // Only execute a limited number of callbacks, to give the actor a chance
            // to update its externally visible state in OperationCompleted() before 
            // more updates are made by more callbacks
            int callbacksToExecute = _pendingCallbackCount;

            using (new SynchronizationContextSetter(this))
            {
                OperationStarted();
                for (int i = 0; i < callbacksToExecute; i++)
                {
                    _pendingCallbacks.TryDequeue(out CallbackAndState callbackAndState);
                    callbackAndState.Execute();
                    // Don't decrement _pendingCallbackCount here, because that could cause
                    // another task to be started and executed while this method is still
                    // running
                }
                OperationCompleted();
            }

            // If more callbacks have been posted while we were executing pending ones,
            // the Post method never saw a pemnding count of 1 and thus it is up to this
            // method to start another task to execute callbacks asynchronously
            if (Interlocked.Add(ref _pendingCallbackCount, -callbacksToExecute) > 0)
            {
                StartTaskToExecuteCallbacks();
            }
        }
    }
}
