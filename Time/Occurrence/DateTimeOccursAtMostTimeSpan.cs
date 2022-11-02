using System;

namespace ShUtilities.Time.Occurrence
{
    public struct DateTimeOccursAtMostTimeSpan
    {
        private readonly DateTime _base;
        private readonly TimeSpan _diff;

        public DateTimeOccursAtMostTimeSpan(DateTime @base, TimeSpan diff)
        {
            _base = @base;
            _diff = diff;
        }

        public bool After(DateTime other) => _base - other <= _diff;
        public bool AfterToday() => _base - TimeProvider.Default.Now.Date <= _diff;

        public bool Before(DateTime other) => other - _base <= _diff;
        public bool BeforeToday() => TimeProvider.Default.Now.Date - _base <= _diff;

    }
}