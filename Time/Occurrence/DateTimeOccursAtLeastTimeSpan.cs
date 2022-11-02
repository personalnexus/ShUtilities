using System;

namespace ShUtilities.Time.Occurrence
{
    public struct DateTimeOccursAtLeastTimeSpan
    {
        private readonly DateTime _base;
        private readonly TimeSpan _diff;

        public DateTimeOccursAtLeastTimeSpan(DateTime @base, TimeSpan diff)
        {
            _base = @base;
            _diff = diff;
        }

        public bool After(DateTime other) => _base - other >= _diff;
        public bool AfterToday() => _base - TimeProvider.Default.Now.Date >= _diff;

        public bool Before(DateTime other) => other - _base >= _diff;
        public bool BeforeToday() => TimeProvider.Default.Now.Date - _base >= _diff;
    }
}