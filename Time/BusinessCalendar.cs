using ShUtilities.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ShUtilities.Time
{
    /// <summary>
    /// Default implementation of <see cref="IBusinessCalendar"/> containing the given holidays (or none if the default constructor is used)
    /// </summary>
    public class BusinessCalendar : IBusinessCalendar, IEnumerable<DateTime>
    {
        public BusinessCalendar(): this(Enumerable.Empty<DateTime>())
        {
        }

        public BusinessCalendar(params DateTime[] holidays): this(holidays as IEnumerable<DateTime>)
        {
        }

        public BusinessCalendar(IEnumerable<DateTime> holidays)
        {
            _holidays = holidays.Select(x => x.Date).ToHashSet();
        }

        private readonly ISet<DateTime> _holidays;

        public void Add(DateTime holiday)
        {
            _holidays.Add(holiday.Date);
        }

        public bool Remove(DateTime holiday)
        {
            bool result = _holidays.Remove(holiday.Date);
            return result;
        }

        // IBusinessCalendar

        public bool IsHoliday(DateTime date)
        {
            bool result = _holidays.Contains(date.Date);
            return result;
        }

        // IEnumerable<DateTime>
        
        public IEnumerator<DateTime> GetEnumerator() => _holidays.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _holidays.GetEnumerator();
    }
}
