using System;

namespace ShUtilities.Time.Occurrence
{
    public readonly ref struct DateTimeOccursExactlyDouble
    {
        private readonly DateTime _base;
        private readonly double _diff;

        public DateTimeOccursExactlyDouble(DateTime @base, double diff)
        {
            _base = @base;
            _diff = diff;
        }

        public readonly DateTimeOccursExactlyTimeSpan Days => new DateTimeOccursExactlyTimeSpan(_base, TimeSpan.FromDays(_diff));
        public readonly DateTimeOccursExactlyTimeSpan Hours => new DateTimeOccursExactlyTimeSpan(_base, TimeSpan.FromHours(_diff));
        public readonly DateTimeOccursExactlyTimeSpan Minutes => new DateTimeOccursExactlyTimeSpan(_base, TimeSpan.FromMinutes(_diff));
        public readonly DateTimeOccursExactlyTimeSpan Seconds => new DateTimeOccursExactlyTimeSpan(_base, TimeSpan.FromSeconds(_diff));
        public readonly DateTimeOccursExactlyTimeSpan Milliseconds => new DateTimeOccursExactlyTimeSpan(_base, TimeSpan.FromMilliseconds(_diff));
    }
}