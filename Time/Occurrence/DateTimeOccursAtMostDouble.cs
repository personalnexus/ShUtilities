using System;

namespace ShUtilities.Time.Occurrence
{
    public readonly ref struct DateTimeOccursAtMostDouble
    {
        private readonly DateTime _base;
        private readonly double _diff;

        public DateTimeOccursAtMostDouble(DateTime @base, double diff)
        {
            _base = @base;
            _diff = diff;
        }

        public readonly DateTimeOccursAtMostTimeSpan Days => new DateTimeOccursAtMostTimeSpan(_base, TimeSpan.FromDays(_diff));
        public readonly DateTimeOccursAtMostTimeSpan Hours => new DateTimeOccursAtMostTimeSpan(_base, TimeSpan.FromHours(_diff));
        public readonly DateTimeOccursAtMostTimeSpan Minutes => new DateTimeOccursAtMostTimeSpan(_base, TimeSpan.FromMinutes(_diff));
        public readonly DateTimeOccursAtMostTimeSpan Seconds => new DateTimeOccursAtMostTimeSpan(_base, TimeSpan.FromSeconds(_diff));
        public readonly DateTimeOccursAtMostTimeSpan Milliseconds => new DateTimeOccursAtMostTimeSpan(_base, TimeSpan.FromMilliseconds(_diff));
    }
}