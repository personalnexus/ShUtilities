using System;

namespace ShUtilities.Time
{
    public interface ITimerService
    {
        ITimerInterval SubscribeInterval(TimeSpan interval, Action<ITimerService, ITimerInterval> callback, IActivitySchedule schedule);
        ITimerSlot SubscribeSlot(TimeSpan slot, Action<ITimerService, ITimerSlot> callback, IActivitySchedule schedule);
    }
}
