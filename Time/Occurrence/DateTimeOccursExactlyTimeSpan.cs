using System;

namespace ShUtilities.Time.Occurrence
{
    public readonly ref struct DateTimeOccursExactlyTimeSpan
    {
        private readonly DateTime _base;
        private readonly TimeSpan _diff;

        public DateTimeOccursExactlyTimeSpan(DateTime @base, TimeSpan diff)
        {
            _base = @base;
            _diff = diff;
        }

        public readonly bool After(DateTime other) => _base - other == _diff;
        public readonly bool AfterToday() => _base - TimeProvider.Default.Now.Date == _diff;

        public readonly bool Before(DateTime other) => other - _base == _diff;
        public readonly bool BeforeToday() => TimeProvider.Default.Now.Date - _base == _diff;
    }
}