using System;

namespace ShUtilities.Time.Occurrence
{
    public struct DateTimeOccursAtLeastDouble
    {
        private readonly DateTime _base;
        private readonly double _diff;

        public DateTimeOccursAtLeastDouble(DateTime @base, double diff)
        {
            _base = @base;
            _diff = diff;
        }

        public DateTimeOccursAtLeastTimeSpanBusinessCalendar BusinessDays(IBusinessCalendar accordingTo) => new DateTimeOccursAtLeastTimeSpanBusinessCalendar(_base, TimeSpan.FromDays(_diff), accordingTo);
        public DateTimeOccursAtLeastTimeSpan Days => new DateTimeOccursAtLeastTimeSpan(_base, TimeSpan.FromDays(_diff));
        public DateTimeOccursAtLeastTimeSpan Hours => new DateTimeOccursAtLeastTimeSpan(_base, TimeSpan.FromHours(_diff));
        public DateTimeOccursAtLeastTimeSpan Minutes => new DateTimeOccursAtLeastTimeSpan(_base, TimeSpan.FromMinutes(_diff));
        public DateTimeOccursAtLeastTimeSpan Seconds => new DateTimeOccursAtLeastTimeSpan(_base, TimeSpan.FromSeconds(_diff));
        public DateTimeOccursAtLeastTimeSpan Milliseconds => new DateTimeOccursAtLeastTimeSpan(_base, TimeSpan.FromMilliseconds(_diff));
    }
}