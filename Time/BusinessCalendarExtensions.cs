using System;

namespace ShUtilities.Time
{
    /// <summary>
    /// Extension methods for <see cref="DateTime"/> using information from an <see cref="IBusinessCalendar"/>
    /// </summary>
    public static class BusinessCalendarExtensions
    {
        public static DateTime AddBusinessDays(this DateTime date, int numberOfDays, IBusinessCalendar calendar)
        {
            int increment = Math.Sign(numberOfDays);
            DateTime result = date;
            while (numberOfDays != 0)
            {
                result = result.AddDays(increment);
                if (result.IsBusinessDay(calendar) && !result.IsWeekend())
                {
                    numberOfDays -= increment;
                }
            }
            return result;
        }

        public static bool IsWeekend(this DateTime date)
        {
            bool result = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
            return result;
        }

        public static bool IsBusinessDay(this DateTime date, IBusinessCalendar calendar)
        {
            bool result = calendar.IsBusinessDay(date);
            return result;
        }

    }
}
