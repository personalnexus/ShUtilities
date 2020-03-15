using System;

namespace ShUtilities.Time
{
    public static class TimerServiceExtensions
    {
        public static ITimerInterval SubscribeInterval(this ITimerService timerService,
                                                       TimeSpan interval,
                                                       Action<ITimerService, ITimerInterval> callback) => timerService.SubscribeInterval(interval, callback, AlwaysActiveSchedule.Default);

        public static ITimerSlot SubscribeSlot(this ITimerService timerService,
                                               TimeSpan slot,
                                               Action<ITimerService, ITimerSlot> callback) => timerService.SubscribeSlot(slot, callback, AlwaysActiveSchedule.Default);

    }
}
