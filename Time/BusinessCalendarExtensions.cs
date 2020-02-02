using System;

namespace ShUtilities.Time
{
    /// <summary>
    /// Extension methods for <see cref="DateTime"/> using information from an <see cref="IBusinessCalendar"/>
    /// </summary>
    public static class BusinessCalendarExtensions
    {
        /// <summary>
        /// Returns the business day that is less than or equal to the given date
        /// </summary>
        public static DateTime BusinessDayOnOrBefore(this DateTime date, IBusinessCalendar calendar)
        {
            DateTime result = date.IsBusinessDay(calendar) ? date : date.AddBusinessDays(-1, calendar);
            return result;
        }

        /// <summary>
        /// Returns the business day that is greater than or equal to the given date
        /// </summary>
        public static DateTime BusinessDayOnOrAfter(this DateTime date, IBusinessCalendar calendar)
        {
            DateTime result = date.IsBusinessDay(calendar) ? date : date.AddBusinessDays(1, calendar);
            return result;
        }

        /// <summary>
        /// Returns the last business day in the month of the given date
        /// </summary>
        public static DateTime LastBusinessDayOfMonth(this DateTime date, IBusinessCalendar calendar)
        {
            DateTime firstDayOfNextMonth = new DateTime(date.Year, date.Month, 1).AddMonths(1);
            DateTime result = firstDayOfNextMonth.AddBusinessDays(-1, calendar);
            return result;
        }

        /// <summary>
        /// Returns the first day of the month, if that is a business day, or the first business day thereafter
        /// </summary>
        public static DateTime FirstBusinessDayOfMonth(this DateTime date, IBusinessCalendar calendar)
        {
            DateTime result = date.FirstOfMonth().BusinessDayOnOrAfter(calendar);
            return result;
        }

        /// <summary>
        /// Similar to <see cref="DateTime.AddDays(double)"/>, adds the given number of business days to the given date
        /// </summary>
        public static DateTime AddBusinessDays(this DateTime date, int fullDays, IBusinessCalendar calendar) => DateExtensions.AddSpecialDays(date, fullDays, x => IsBusinessDay(x, calendar));
        
        /// <summary>
        /// Determines whether the given date is 
        /// (a) not a holiday according to the given calendar and 
        /// (b) not a Saturday or Sunday
        /// </summary>
        public static bool IsBusinessDay(this DateTime date, IBusinessCalendar calendar)
        {
            bool result = !date.IsWeekend() &&
                          !calendar.IsHoliday(date);
            return result;
        }
    }
}
