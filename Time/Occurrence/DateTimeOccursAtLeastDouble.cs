using System;

namespace ShUtilities.Time.Occurrence
{
    public readonly ref struct DateTimeOccursAtLeastDouble
    {
        private readonly DateTime _base;
        private readonly double _diff;

        public DateTimeOccursAtLeastDouble(DateTime @base, double diff)
        {
            _base = @base;
            _diff = diff;
        }

        public readonly DateTimeOccursAtLeastTimeSpanBusinessCalendar BusinessDays(IBusinessCalendar accordingTo) => new DateTimeOccursAtLeastTimeSpanBusinessCalendar(_base, TimeSpan.FromDays(_diff), accordingTo);
        public readonly DateTimeOccursAtLeastTimeSpan Days => new DateTimeOccursAtLeastTimeSpan(_base, TimeSpan.FromDays(_diff));
        public readonly DateTimeOccursAtLeastTimeSpan Hours => new DateTimeOccursAtLeastTimeSpan(_base, TimeSpan.FromHours(_diff));
        public readonly DateTimeOccursAtLeastTimeSpan Minutes => new DateTimeOccursAtLeastTimeSpan(_base, TimeSpan.FromMinutes(_diff));
        public readonly DateTimeOccursAtLeastTimeSpan Seconds => new DateTimeOccursAtLeastTimeSpan(_base, TimeSpan.FromSeconds(_diff));
        public readonly DateTimeOccursAtLeastTimeSpan Milliseconds => new DateTimeOccursAtLeastTimeSpan(_base, TimeSpan.FromMilliseconds(_diff));
    }
}