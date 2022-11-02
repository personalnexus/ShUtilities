using System;

namespace ShUtilities.Time.Occurrence
{
    public struct DateTimeOccursExactlyDouble
    {
        private readonly DateTime _base;
        private readonly double _diff;

        public DateTimeOccursExactlyDouble(DateTime @base, double diff)
        {
            _base = @base;
            _diff = diff;
        }

        public DateTimeOccursExactlyTimeSpan Days => new DateTimeOccursExactlyTimeSpan(_base, TimeSpan.FromDays(_diff));
        public DateTimeOccursExactlyTimeSpan Hours => new DateTimeOccursExactlyTimeSpan(_base, TimeSpan.FromHours(_diff));
        public DateTimeOccursExactlyTimeSpan Minutes => new DateTimeOccursExactlyTimeSpan(_base, TimeSpan.FromMinutes(_diff));
        public DateTimeOccursExactlyTimeSpan Seconds => new DateTimeOccursExactlyTimeSpan(_base, TimeSpan.FromSeconds(_diff));
        public DateTimeOccursExactlyTimeSpan Milliseconds => new DateTimeOccursExactlyTimeSpan(_base, TimeSpan.FromMilliseconds(_diff));
    }
}