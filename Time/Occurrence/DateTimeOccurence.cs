using System;

namespace ShUtilities.Time.Occurrence
{
    public readonly ref struct DateTimeOccurs
    {
        private readonly DateTime _base;

        public DateTimeOccurs(DateTime baseDateTime)
        {
            _base = baseDateTime;
        }

        /// <example>
        ///  X.Occurs().AtLeast(10).Days.Before(Y)
        /// </example>
        public readonly DateTimeOccursAtLeastDouble AtLeast(double diff) => new DateTimeOccursAtLeastDouble(_base, diff);

        /// <example>
        ///  X.Occurs().AtLeast(TimeSpan.FromHours(12)).Before(Y)
        /// </example>
        public readonly DateTimeOccursAtLeastTimeSpan AtLeast(TimeSpan diff) => new DateTimeOccursAtLeastTimeSpan(_base, diff);

        /// <example>
        ///  X.Occurs().AtMost(5).Days.Before(Y)
        /// </example>
        public readonly DateTimeOccursAtMostDouble AtMost(double diff) => new DateTimeOccursAtMostDouble(_base, diff);

        /// <example>
        ///  X.Occurs().AtMost(TimeSpan.FromHours(12)).Before(Y)
        /// </example>
        public readonly DateTimeOccursAtMostTimeSpan AtMost(TimeSpan diff) => new DateTimeOccursAtMostTimeSpan(_base, diff);

        /// <example>
        ///  X.Occurs().Exactly(5).Days.Before(Y)
        /// </example>
        public readonly DateTimeOccursExactlyDouble Exactly(double diff) => new DateTimeOccursExactlyDouble(_base, diff);

        /// <example>
        ///  X.Occurs().Exactly(TimeSpan.FromHours(12)).Before(Y)
        /// </example>
        public readonly DateTimeOccursExactlyTimeSpan Exactly(TimeSpan diff) => new DateTimeOccursExactlyTimeSpan(_base, diff);
    }
}