using System;

namespace ShUtilities.Time
{
    /// <summary>
    /// Interface that provides information on whether a given date is a business day or not
    /// </summary>
    public interface IBusinessCalendar
    {
        bool IsHoliday(DateTime date);
    }
}
