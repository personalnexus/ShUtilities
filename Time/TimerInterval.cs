using System;

namespace ShUtilities.Time
{
    internal class TimerInterval : ITimerInterval
    {
        public TimerInterval(TimerService timerService,
                             TimeSpan interval,
                             Action<ITimerService, ITimerInterval> callback,
                             IActivitySchedule schedule)
        {
            TimerService = timerService;
            Interval = interval;
            Callback = callback;
            Schedule = schedule;
            NextStopInTicks = TimeProvider.Default.Ticks + _intervalInTicks;
        }

        internal TimerService TimerService { get; set; }
        public TimeSpan Interval
        {
            get => _interval;
            set
            {
                _interval = value;
                _intervalInTicks = (long)(value.TotalSeconds * TimeProvider.Default.TicksPerSecond);
            }
        }

        private long _intervalInTicks;
        private TimeSpan _interval;

        public Action<ITimerService, ITimerInterval> Callback { get; set; }
        public IActivitySchedule Schedule { get; set; }

        public void Unsubscribe() => TimerService.UnsubscribeInterval(this);

        internal void Check(DateTime now, long nowInTicks)
        {
            if (nowInTicks > NextStopInTicks)
            {
                // If the next interval would be in the past, because more than one interval as elapsed since the
                // last check, just use Now as the next interval
                NextStopInTicks = Math.Min(NextStopInTicks + _intervalInTicks, nowInTicks);
                if (Schedule.IsActiveTime(now))
                {
                    Callback(TimerService, this);
                }
            }
        }

        // Calculating in ticks instead of with absoulte times makes the interval more robust
        // with regards to daylight saving time and other changes to the clock.
        internal long NextStopInTicks { get; set; }
    }
}