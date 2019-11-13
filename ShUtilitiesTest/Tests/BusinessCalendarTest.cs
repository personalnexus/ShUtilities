using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Time;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class BusinessCalendarTest: IBusinessCalendar
    {
        [TestMethod]
        public void IsBusinessDay()
        {
            Assert.IsFalse(WeekendDay.IsBusinessDay(this));
            Assert.IsTrue(BusinessDay.IsBusinessDay(this));
        }

        [TestMethod]
        public void AddBusinessDays()
        {
            Assert.AreEqual(BusinessDay, BusinessDay.AddBusinessDays(0, this));
            Assert.AreEqual(WeekendDay, WeekendDay.AddBusinessDays(0, this));
            Assert.AreEqual(new DateTime(2019, 10, 17), WeekendDay.AddBusinessDays(-1, this));
            Assert.AreEqual(new DateTime(2019, 10, 24), new DateTime(2019, 10, 19).AddBusinessDays(2, this));
            Assert.AreEqual(new DateTime(2020, 01, 02), NewYears.AddBusinessDays(1, this));
        }

        [TestMethod]
        public void LastBusinessDayOfMonth()
        {
            Assert.AreEqual(new DateTime(2019, 12, 30), BusinessDayInDecember.LastBusinessDayOfMonth(this));
            Assert.AreEqual(new DateTime(2019, 10, 31), BusinessDay.LastBusinessDayOfMonth(this));
            Assert.AreEqual(new DateTime(2019, 10, 31), WeekendDay.LastBusinessDayOfMonth(this));
        }

        [TestMethod]
        public void BusinessDayOnOrAfter()
        {
            Assert.AreEqual(new DateTime(2020, 01, 02), NewYearsEve.BusinessDayOnOrAfter(this));
            Assert.AreEqual(new DateTime(2020, 01, 02), NewYears.BusinessDayOnOrAfter(this));
            Assert.AreEqual(BusinessDayInDecember, BusinessDayInDecember.BusinessDayOnOrAfter(this));
            Assert.AreEqual(BusinessDay, BusinessDay.BusinessDayOnOrAfter(this));
        }

        [TestMethod]
        public void BusinessDayOnOrBefore()
        {
            Assert.AreEqual(new DateTime(2019, 12, 30), NewYearsEve.BusinessDayOnOrBefore(this));
            Assert.AreEqual(new DateTime(2019, 12, 30), NewYears.BusinessDayOnOrBefore(this));
            Assert.AreEqual(BusinessDayInDecember, BusinessDayInDecember.BusinessDayOnOrBefore(this));
            Assert.AreEqual(BusinessDay, BusinessDay.BusinessDayOnOrBefore(this));
        }

        private static readonly DateTime WeekendDay = new DateTime(2019, 10, 19);
        private static readonly DateTime BusinessDay = new DateTime(2019, 10, 17);
        private static readonly DateTime BusinessDayInDecember = new DateTime(2019, 12, 18);
        private static readonly DateTime NewYearsEve = new DateTime(2019, 12, 31);
        private static readonly DateTime NewYears = new DateTime(2020, 01, 01);

        private static readonly DateTime[] Holidays = {
                                                        new DateTime(2019, 10, 18),
                                                        new DateTime(2019, 10, 22),
                                                        new DateTime(2019, 10, 23),
                                                        NewYearsEve,
                                                        NewYears,
                                                      };

        public bool IsHoliday(DateTime date)
        {
            bool result = Holidays.Contains(date);
            return result;
        }
    }
}
