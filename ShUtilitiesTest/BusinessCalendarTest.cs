using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Time;

namespace ShUtilitiesTest
{
    [TestClass]
    public class BusinessCalendarTest: IBusinessCalendar
    {
        [TestMethod]
        public void IsWeekend()
        {
            Assert.IsTrue(WeekendDay.IsWeekend());
            Assert.IsFalse(BusinessDay.IsWeekend());
        }

        [TestMethod]
        public void AddBusinessDays()
        {
            Assert.AreEqual(BusinessDay, BusinessDay.AddBusinessDays(0, this));
            Assert.AreEqual(WeekendDay, WeekendDay.AddBusinessDays(0, this));
            Assert.AreEqual(new DateTime(2019, 10, 17), WeekendDay.AddBusinessDays(-1, this));
            Assert.AreEqual(new DateTime(2019, 10, 24), new DateTime(2019, 10, 19).AddBusinessDays(2, this));
        }

        private static readonly DateTime WeekendDay = new DateTime(2019, 10, 19);
        private static readonly DateTime BusinessDay = new DateTime(2019, 10, 17);

        private static readonly DateTime[] Holidays = {
                                                        new DateTime(2019, 10, 18),
                                                        new DateTime(2019, 10, 22),
                                                        new DateTime(2019, 10, 23)
                                                      };

        public bool IsBusinessDay(DateTime date)
        {
            bool result = !Holidays.Contains(date);
            return result;
        }
    }
}
