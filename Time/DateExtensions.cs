using System;
using ShUtilities.Time.Occurrence;

namespace ShUtilities.Time
{
    /// <summary>
    /// Convenience methods for comparing dates
    /// </summary>
    public static class DateExtensions
    {
        public static DateTimeOccurs Occurs(this DateTime date) => new DateTimeOccurs(date);

        /// <summary>
        /// Returns the first day of the month that <paramref name="date"/> is in.
        /// </summary>
        public static DateTime FirstOfMonth(this DateTime date)
        {
            var result = new DateTime(date.Year, date.Month, 1);
            return result;
        }

        /// <summary>
        /// Returns the last day of the month that <paramref name="date"/> is in.
        /// </summary>
        public static DateTime LastOfMonth(this DateTime date)
        {
            var firstDayOfNextMonth = new DateTime(date.Year, date.Month, 1).AddMonths(1);
            DateTime result = firstDayOfNextMonth.AddDays(-1);
            return result;
        }

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

        public static DateTime AddWeekdays(this DateTime date, int fullDays) => AddSpecialDays(date, fullDays, IsWeekday);

        internal static DateTime AddSpecialDays(this DateTime date, int fullDays, Func<DateTime, bool> isSpecialDay)
        {
            int increment = Math.Sign(fullDays);
            DateTime result = date;
            while (fullDays != 0)
            {
                result = result.AddDays(increment);
                if (isSpecialDay(result))
                {
                    fullDays -= increment;
                }
            }
            return result;
        }

        public static bool IsWeekday(this DateTime date) => !IsWeekend(date);

        /// <summary>
        /// Determines whether the given date a Saturday or Sunday
        /// </summary>
        public static bool IsWeekend(this DateTime date)
        {
            bool result = date.DayOfWeek == DayOfWeek.Saturday ||
                          date.DayOfWeek == DayOfWeek.Sunday;
            return result;
        }
    }
}
