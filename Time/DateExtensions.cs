using System;

namespace ShUtilities.Time
{
    /// <summary>
    /// Convenience methods for comparing dates
    /// </summary>
    public static class DateExtensions
    {
        /// <summary>
        /// Determines whether now is after the base date
        /// </summary>
        public static bool IsAfterNow(this DateTime baseDate)
        {
            return IsBefore(TimeProvider.Default.Now, baseDate);
        }

        /// <summary>
        /// Determines whether now is after the base date by the given TimeSpan
        /// </summary>
        public static bool IsAfterNow(this DateTime baseDate, TimeSpan diff)
        {
            return IsBefore(TimeProvider.Default.Now, baseDate, diff);
        }

        /// <summary>
        /// Determines whether the other date is after the base date
        /// </summary>
        public static bool IsAfter(this DateTime baseDate, DateTime otherDate)
        {
            return IsBefore(otherDate, baseDate, TimeSpan.Zero);
        }

        /// <summary>
        /// Determines whether the other date is after the base date by the given TimeSpan
        /// </summary>
        public static bool IsAfter(this DateTime baseDate, DateTime otherDate, TimeSpan diff)
        {
            return IsBefore(otherDate, baseDate, diff);
        }

        /// <summary>
        /// Determines whether now is before the base date
        /// </summary>
        public static bool IsBeforeNow(this DateTime baseDate)
        {
            return IsBefore(baseDate, TimeProvider.Default.Now, TimeSpan.Zero);
        }

        /// <summary>
        /// Determines whether now is before the base date by the given TimeSpan
        /// </summary>
        public static bool IsBeforeNow(this DateTime baseDate, TimeSpan diff)
        {
            return IsBefore(baseDate, TimeProvider.Default.Now, diff);
        }

        /// <summary>
        /// Determines whether the other date is before the base date
        /// </summary>
        public static bool IsBefore(this DateTime baseDate, DateTime otherDate)
        {
            return IsBefore(baseDate, otherDate, TimeSpan.Zero);
        }

        /// <summary>
        /// Determines whether the other date is before the base date by the given TimeSpan
        /// </summary>
        public static bool IsBefore(this DateTime baseDate, DateTime otherDate, TimeSpan diff)
        {
            bool result = baseDate + diff < otherDate;
            return result;
        }
    }
}
