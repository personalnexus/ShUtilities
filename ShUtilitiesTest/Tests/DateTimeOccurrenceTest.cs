using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class DateTimeOccurrenceTest
    {

        // After - AtLeast

        [TestMethod]
        public void OccursAtLeastAfterToday_InFiveDays_IsFalse()
        {
            TimeProvider.Default.Now.Date.AddDays(5).Occurs().AtLeast(10).Days.AfterToday().Should().BeFalse();
        }

        [TestMethod]
        public void OccursAtLeastAfterToday_InTenDays_IsTrue()
        {
            TimeProvider.Default.Now.Date.AddDays(10).Occurs().AtLeast(10).Days.AfterToday().Should().BeTrue();
        }

        [TestMethod]
        public void OccursAtLeastAfterToday_InTwentyDays_IsTrue()
        {
            TimeProvider.Default.Now.Date.AddDays(20).Occurs().AtLeast(10).Days.AfterToday().Should().BeTrue();
        }

        // After - AtMost

        [TestMethod]
        public void OccursAtMostAfterToday_InFiveDays_IsTrue()
        {
            TimeProvider.Default.Now.Date.AddDays(5).Occurs().AtMost(10).Days.AfterToday().Should().BeTrue();
        }

        [TestMethod]
        public void OccursAtMostAfterToday_TenDaysAgo_IsTrue()
        {
            TimeProvider.Default.Now.Date.AddDays(10).Occurs().AtMost(10).Days.AfterToday().Should().BeTrue();
        }

        [TestMethod]
        public void OccursAtMostAfterToday_TwentyDaysAgo_IsFalse()
        {
            TimeProvider.Default.Now.Date.AddDays(20).Occurs().AtMost(10).Days.AfterToday().Should().BeFalse();
        }

        // After - Exactly

        [TestMethod]
        public void OccursExactlyAfterToday_InFiveDays_IsFalse()
        {
            TimeProvider.Default.Now.Date.AddDays(5).Occurs().Exactly(10).Days.AfterToday().Should().BeFalse();
        }

        [TestMethod]
        public void OccursExactlyAfterToday_InTenDays_IsTrue()
        {
            TimeProvider.Default.Now.Date.AddDays(10).Occurs().Exactly(10).Days.AfterToday().Should().BeTrue();
        }

        [TestMethod]
        public void OccursExactlyAfterToday_InTwentyDays_IsFalse()
        {
            TimeProvider.Default.Now.Date.AddDays(20).Occurs().Exactly(10).Days.AfterToday().Should().BeFalse();
        }

        // Before - AtLeast

        [TestMethod]
        public void OccursAtLeastBeforeToday_FiveDaysAgo_IsFalse()
        {
            TimeProvider.Default.Now.Date.AddDays(-5).Occurs().AtLeast(10).Days.BeforeToday().Should().BeFalse();
        }

        [TestMethod]
        public void OccursAtLeastBeforeToday_TenDaysAgo_IsTrue()
        {
            TimeProvider.Default.Now.Date.AddDays(-10).Occurs().AtLeast(10).Days.BeforeToday().Should().BeTrue();
        }

        [TestMethod]
        public void OccursAtLeastBeforeToday_TwentyDaysAgo_IsTrue()
        {
            TimeProvider.Default.Now.Date.AddDays(-20).Occurs().AtLeast(10).Days.BeforeToday().Should().BeTrue();
        }

        // Before - AtMost

        [TestMethod]
        public void OccursAtMostBeforeToday_FiveDaysAgo_IsTrue()
        {
            TimeProvider.Default.Now.Date.AddDays(-5).Occurs().AtMost(10).Days.BeforeToday().Should().BeTrue();
        }

        [TestMethod]
        public void OccursAtMostBeforeToday_TenDaysAgo_IsTrue()
        {
            TimeProvider.Default.Now.Date.AddDays(-10).Occurs().AtMost(10).Days.BeforeToday().Should().BeTrue();
        }

        [TestMethod]
        public void OccursAtMostBeforeToday_TwentyDaysAgo_IsFalse()
        {
            TimeProvider.Default.Now.Date.AddDays(-20).Occurs().AtMost(10).Days.BeforeToday().Should().BeFalse();
        }

        // Before - Exactly

        [TestMethod]
        public void OccursExactlyBeforeToday_FiveDaysAgo_IsFalse()
        {
            TimeProvider.Default.Now.Date.AddDays(-5).Occurs().Exactly(10).Days.BeforeToday().Should().BeFalse();
        }

        [TestMethod]
        public void OccursExactlyBeforeToday_TenDaysAgo_IsTrue()
        {
            TimeProvider.Default.Now.Date.AddDays(-10).Occurs().Exactly(10).Days.BeforeToday().Should().BeTrue();
        }

        [TestMethod]
        public void OccursExactlyBeforeToday_TwentyDaysAgo_IsFalse()
        {
            TimeProvider.Default.Now.Date.AddDays(-20).Occurs().Exactly(10).Days.BeforeToday().Should().BeFalse();
        }

        [TestMethod]
        public void OccursAtLeastBusinessDaysAfter_ThreeDaysLater_IsTrue()
        {
            Day7.Occurs().AtLeast(3).BusinessDays(accordingTo: HolidayCalendar).After(Day1).Should().BeTrue();
        }

        [TestMethod]
        public void OccursAtLeastBusinessDaysAfter_SixDaysLater_IsFalse()
        {
            Day7.Occurs().AtLeast(6).BusinessDays(accordingTo: HolidayCalendar).After(Day1).Should().BeFalse();
        }

        private static readonly DateTime Day1 = new DateTime(2000, 11, 1);
        private static readonly DateTime Day7 = new DateTime(2000, 11, 8); // two days on a weekend and one holiday later
        private static readonly IBusinessCalendar HolidayCalendar = new BusinessCalendar(new DateTime(2000, 11, 3));
    }
}
