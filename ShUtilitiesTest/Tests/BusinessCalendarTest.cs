using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Time;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class BusinessCalendarTest
    {
        [TestMethod]
        public void IsBusinessDay()
        {
            Assert.IsFalse(WeekendDay.IsBusinessDay(Calendar));
            Assert.IsTrue(BusinessDay.IsBusinessDay(Calendar));
        }

        [TestMethod]
        public void AddBusinessDays()
        {
            Assert.AreEqual(BusinessDay, BusinessDay.AddBusinessDays(0, Calendar));
            Assert.AreEqual(WeekendDay, WeekendDay.AddBusinessDays(0, Calendar));
            Assert.AreEqual(new DateTime(2019, 10, 17), WeekendDay.AddBusinessDays(-1, Calendar));
            Assert.AreEqual(new DateTime(2019, 10, 24), new DateTime(2019, 10, 19).AddBusinessDays(2, Calendar));
            Assert.AreEqual(new DateTime(2020, 01, 02), NewYears.AddBusinessDays(1, Calendar));
        }

        [TestMethod]
        public void FirstBusinessDayOfMonth()
        {
            Assert.AreEqual(new DateTime(2020, 01, 02), NewYears.FirstBusinessDayOfMonth(Calendar));
            Assert.AreEqual(new DateTime(2020, 02, 03), NewYears.AddMonths(1).FirstBusinessDayOfMonth(Calendar)); // because Feb 1st and 2nd are a weekend
            Assert.AreEqual(new DateTime(2020, 04, 01), NewYears.AddMonths(3).FirstBusinessDayOfMonth(Calendar));
        }

        [TestMethod]
        public void LastBusinessDayOfMonth()
        {
            Assert.AreEqual(new DateTime(2019, 12, 30), BusinessDayInDecember.LastBusinessDayOfMonth(Calendar));
            Assert.AreEqual(new DateTime(2019, 10, 31), BusinessDay.LastBusinessDayOfMonth(Calendar));
            Assert.AreEqual(new DateTime(2019, 10, 31), WeekendDay.LastBusinessDayOfMonth(Calendar));
        }

        [TestMethod]
        public void BusinessDayOnOrAfter()
        {
            Assert.AreEqual(new DateTime(2020, 01, 02), NewYearsEve.BusinessDayOnOrAfter(Calendar));
            Assert.AreEqual(new DateTime(2020, 01, 02), NewYears.BusinessDayOnOrAfter(Calendar));
            Assert.AreEqual(BusinessDayInDecember, BusinessDayInDecember.BusinessDayOnOrAfter(Calendar));
            Assert.AreEqual(BusinessDay, BusinessDay.BusinessDayOnOrAfter(Calendar));
        }

        [TestMethod]
        public void BusinessDayOnOrBefore()
        {
            Assert.AreEqual(new DateTime(2019, 12, 30), NewYearsEve.BusinessDayOnOrBefore(Calendar));
            Assert.AreEqual(new DateTime(2019, 12, 30), NewYears.BusinessDayOnOrBefore(Calendar));
            Assert.AreEqual(BusinessDayInDecember, BusinessDayInDecember.BusinessDayOnOrBefore(Calendar));
            Assert.AreEqual(BusinessDay, BusinessDay.BusinessDayOnOrBefore(Calendar));
        }

        private static readonly DateTime WeekendDay = new DateTime(2019, 10, 19);
        private static readonly DateTime BusinessDay = new DateTime(2019, 10, 17);
        private static readonly DateTime BusinessDayInDecember = new DateTime(2019, 12, 18);
        private static readonly DateTime NewYearsEve = new DateTime(2019, 12, 31);
        private static readonly DateTime NewYears = new DateTime(2020, 01, 01);

        private static readonly BusinessCalendar Calendar = new BusinessCalendar
        {
            new DateTime(2019, 10, 18),
            new DateTime(2019, 10, 22),
            new DateTime(2019, 10, 23),
            NewYearsEve,
            NewYears,
        };
    }
}
