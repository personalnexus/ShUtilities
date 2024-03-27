using ShUtilities.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShUtilities.Threading.DelayedActions
{
    public class DelayedAction : IDelayedAction
    {
        public DelayedAction(Action action, DateTime executionTime)
        {
            _action = action;
            _executionTime = executionTime;
        }

        private readonly Action _action;
        private readonly DateTime _executionTime;
        private long _isCancelled; // initial value of 0 means, not cancelled. Set to 1 when being cancelled.

        public TimeSpan GetTimeUntilExecution(ITimeProvider timeProvider) => _executionTime - timeProvider.Now;

        public bool TryCancel() => Interlocked.Exchange(ref _isCancelled, 1) == 0;

        public void Execute() => _action();
    }
}
