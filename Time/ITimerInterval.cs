using System;

namespace ShUtilities.Time
{
    public interface ITimerInterval
    {
        TimeSpan Interval { get; set; }
        Action<ITimerService, ITimerInterval> Callback { get; set; }
        IActivitySchedule Schedule { get; set; }
        void Unsubscribe();
    }
}