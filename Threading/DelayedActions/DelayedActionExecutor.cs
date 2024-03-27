using ShUtilities.Threading.ActionExecutors;
using ShUtilities.Time;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShUtilities.Threading.DelayedActions
{
    public class DelayedActionExecutor : IDisposable
    {
        public DelayedActionExecutor(ITimeProvider timeProvider,
                                     IActionExecutor actionExecutor,
                                     TimeSpan delay)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _pendingActions = new BlockingCollection<IDelayedAction>();
            _timeProvider = timeProvider;
            _delay = delay;
            _actionExecutor = actionExecutor;
        }

        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly BlockingCollection<IDelayedAction> _pendingActions;
        private readonly ITimeProvider _timeProvider;
        private readonly TimeSpan _delay;
        private readonly IActionExecutor _actionExecutor;

        public void Dispose() => _cancellationTokenSource.Cancel();

        public void Start() => _actionExecutor.Execute(CheckQueue, _cancellationTokenSource.Token);

        /// <summary>
        /// Loop until cancellation to get the next action and execute it when it's time is up
        /// </summary>
        private void CheckQueue()
        {
            while (_pendingActions.TryTake(out IDelayedAction delayedAction, Timeout.Infinite, _cancellationTokenSource.Token))
            {
                TimeSpan timeUntilExecution = delayedAction.GetTimeUntilExecution(_timeProvider);
                if (timeUntilExecution > TimeSpan.Zero)
                {
                    //
                    // We can sleep here knowing that all actions in the queue behind the current one have later execution times
                    //
                    Thread.Sleep(timeUntilExecution);
                }
                //
                // TryCancel returns true when we cancelled the action and the action had not already been cancelled before
                //
                if (delayedAction.TryCancel())
                {
                    delayedAction.Execute();
                }
            }
        }

        /// <summary>
        /// Wraps an <see cref="Action"/> into an <see cref="ICancelable"/> to cancel the action in a thread-safe fashion.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public ICancelable Enqueue(Action action)
        {
            var result = new DelayedAction(action, executionTime: _timeProvider.Now + _delay);
            Enqueue(result);
            return result;
        }

        /// <summary>
        /// If you already have a thread-safe class implementing <see cref="IDelayedAction"/>, use this overload and save the allocation of a wrapper for <see cref="Action"/>
        /// </summary>
        /// <param name="delayedAction"></param>
        public void Enqueue(IDelayedAction delayedAction) => _pendingActions.Add(delayedAction);

        public int PendingCount => _pendingActions.Count;
    }
}
