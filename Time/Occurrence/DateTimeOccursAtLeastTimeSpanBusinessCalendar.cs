using System;

namespace ShUtilities.Time.Occurrence
{
    public readonly ref struct DateTimeOccursAtLeastTimeSpanBusinessCalendar
    {
        private readonly DateTime _base;
        private readonly TimeSpan _diff;
        private readonly IBusinessCalendar _calendar;

        public DateTimeOccursAtLeastTimeSpanBusinessCalendar(DateTime @base, TimeSpan diff, IBusinessCalendar calendar)
        {
            _base = @base;
            _diff = diff;
            _calendar = calendar;
        }

        public readonly bool After(DateTime other) => _base >= AddBusinessDaysDiff(other);
        public readonly bool AfterToday() => After(TimeProvider.Default.Now.Date);

        public readonly bool Before(DateTime other) => _base <= AddBusinessDaysDiff(other);
        public readonly bool BeforeToday() => Before(TimeProvider.Default.Now.Date);

        private readonly DateTime AddBusinessDaysDiff(DateTime other)
        {
            int fullDaysInDiff = _diff.Days;
            long partialDaysInDiff = _diff.Ticks - (fullDaysInDiff * TimeSpan.TicksPerDay);
            // Convert 'other' to a business day, but make sure to add the partial days contained in _diff first,
            // so we definitely end up on a business day after the call to AddBusinessDays().
            DateTime otherBusinessDays = other
                .AddTicks(partialDaysInDiff)
                .AddBusinessDays(fullDaysInDiff, _calendar);
            return otherBusinessDays;
        }
    }
}