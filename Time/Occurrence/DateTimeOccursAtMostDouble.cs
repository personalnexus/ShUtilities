using System;

namespace ShUtilities.Time.Occurrence
{
    public struct DateTimeOccursAtMostDouble
    {
        private readonly DateTime _base;
        private readonly double _diff;

        public DateTimeOccursAtMostDouble(DateTime @base, double diff)
        {
            _base = @base;
            _diff = diff;
        }

        public DateTimeOccursAtMostTimeSpan Days => new DateTimeOccursAtMostTimeSpan(_base, TimeSpan.FromDays(_diff));
        public DateTimeOccursAtMostTimeSpan Hours => new DateTimeOccursAtMostTimeSpan(_base, TimeSpan.FromHours(_diff));
        public DateTimeOccursAtMostTimeSpan Minutes => new DateTimeOccursAtMostTimeSpan(_base, TimeSpan.FromMinutes(_diff));
        public DateTimeOccursAtMostTimeSpan Seconds => new DateTimeOccursAtMostTimeSpan(_base, TimeSpan.FromSeconds(_diff));
        public DateTimeOccursAtMostTimeSpan Milliseconds => new DateTimeOccursAtMostTimeSpan(_base, TimeSpan.FromMilliseconds(_diff));
    }
}