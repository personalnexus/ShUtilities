using System;
using System.Collections.Immutable;

namespace ShUtilities.Time
{
    public class TimerService : ITimerService
    {
        public TimerService()
        {
            _intervals = ImmutableList<TimerInterval>.Empty;
        }

        private ImmutableList<TimerInterval> _intervals;


        public void Check()
        {
            // Use the same definition of "now" for all checks, so intervals move in unison
            long nowInTicks = Time.TimeProvider.Default.Ticks;
            DateTime now = Time.TimeProvider.Default.Now;
            // Because _intervals is immutable, we can enumerate it even callbacks remove
            // items during enumeration
            foreach (TimerInterval interval in _intervals)
            {
                interval.Check(now, nowInTicks);
            }
        }

        // ITimerService

        public ITimerInterval SubscribeInterval(TimeSpan interval, Action<ITimerService, ITimerInterval> callback, IActivitySchedule schedule)
        {
            TimerInterval result = new TimerInterval(this, interval, callback, schedule);
            _intervals = _intervals.Add(result);
            return result;
        }

        public ITimerSlot SubscribeSlot(TimeSpan slot, Action<ITimerService, ITimerSlot> callback, IActivitySchedule schedule)
        {
            throw new NotImplementedException();
        }

        // Internals for ITimerInterval, ITimerSlot

        internal void UnsubscribeInterval(TimerInterval timerInterval)
        {
            _intervals = _intervals.Remove(timerInterval);
        }
    }
}
