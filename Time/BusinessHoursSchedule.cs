using System;

namespace ShUtilities.Time
{
    public class BusinessHoursSchedule : IActivitySchedule
    {
        public BusinessHoursSchedule(IBusinessCalendar calendar, TimeSpan begin, TimeSpan end)
        {
            var oneDay = TimeSpan.FromDays(1);
            if (begin >= oneDay)
            {
                throw new ArgumentException($"{nameof(begin)} must be a time of day and cannot be greater than or equal to 1.");
            }
            if (end > oneDay)
            {
                throw new ArgumentException($"{nameof(end)} must be a time of day and cannot be greater than 1.");
            }
            if (begin > end)
            {
                throw new ArgumentException($"{nameof(begin)} must be less than {nameof(end)}");
            }
            Calendar = calendar;
            Begin = begin;
            End = end;
        }

        public IBusinessCalendar Calendar { get; }
        public TimeSpan Begin { get; }
        public TimeSpan End { get; }

        public bool IsActiveTime(DateTime time)
        {
            bool result = !Calendar.IsHoliday(time);
            if (result)
            {
                TimeSpan timeOfDay = time.TimeOfDay;
                result = Begin < timeOfDay && timeOfDay < End;
            }
            return result;
        }
    }
}
