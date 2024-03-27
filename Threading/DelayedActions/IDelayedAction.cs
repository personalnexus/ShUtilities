using ShUtilities.Time;
using System;

namespace ShUtilities.Threading.DelayedActions
{
    public interface IDelayedAction : ICancelable
    {
        TimeSpan GetTimeUntilExecution(ITimeProvider timeProvider);
        void Execute();
    }
}