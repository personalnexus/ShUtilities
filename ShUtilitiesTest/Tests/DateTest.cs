using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ShUtilities.Time;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class DateTest
    {
        private static readonly DateTime EarlierDate = new DateTime(2019, 11, 18);
        private static readonly DateTime LaterDate = new DateTime(2019, 11, 19, 18, 30, 00);

        [TestMethod]
        public void IsBefore()
        {
            Assert.IsTrue(EarlierDate.IsBefore(LaterDate));
            Assert.IsTrue(EarlierDate.IsBefore(LaterDate, TimeSpan.FromDays(1)));
            Assert.IsFalse(EarlierDate.IsBefore(LaterDate, TimeSpan.FromDays(2)));
            Assert.IsFalse(LaterDate.IsBefore(EarlierDate));
            Assert.IsFalse(EarlierDate.IsBefore(EarlierDate));
        }

        [TestMethod]
        public void IsAfter()
        {
            Assert.IsTrue(LaterDate.IsAfter(EarlierDate));
            Assert.IsTrue(LaterDate.IsAfter(EarlierDate, TimeSpan.FromDays(1)));
            Assert.IsFalse(LaterDate.IsAfter(EarlierDate, TimeSpan.FromDays(2)));
            Assert.IsFalse(EarlierDate.IsAfter(LaterDate));
            Assert.IsFalse(LaterDate.IsAfter(LaterDate));
        }

        [TestMethod]
        public void ApplyOffsets()
        {
            const string offsetsString = "+2B;-4D;1W";
            var calendar = new BusinessCalendar(new DateTime(2020, 02, 03));

            Assert.IsTrue(DateOffsets.TryParse(offsetsString, calendar, out DateOffsets offsets));
            
            Assert.AreEqual((2, DateOffsetKind.BusinessDays), offsets.Items[0]);
            Assert.AreEqual((-4, DateOffsetKind.Days), offsets.Items[1]);
            Assert.AreEqual((1, DateOffsetKind.Weekdays), offsets.Items[2]);

            Assert.AreEqual(new DateTime(2020, 02, 03), offsets.ApplyTo(new DateTime(2020, 02, 02)));
            Assert.AreEqual(offsetsString[1..], offsets.ToString()); // plus sign is acceptable in input, but not included in output
        }

        [TestMethod]
        public void ValidOffsets()
        {
            Assert.IsTrue(DateOffsets.TryParse("-B;+D;+D;-W", out _));
            Assert.IsTrue(DateOffsets.TryParse("2B", out _));
            Assert.IsTrue(DateOffsets.TryParse("568B;-254D", out _));
        }

        [TestMethod]
        public void InvalidOffsets()
        {
            Assert.IsFalse(DateOffsets.TryParse("x", out _));
            Assert.IsFalse(DateOffsets.TryParse("2", out _));
            Assert.IsFalse(DateOffsets.TryParse("2b", out _));
            Assert.IsFalse(DateOffsets.TryParse("2A", out _));
            Assert.IsFalse(DateOffsets.TryParse("1B;", out _));
            Assert.IsFalse(DateOffsets.TryParse("1B;;2W", out _));
        }

        [TestMethod]
        public void FluentOffsets()
        {
            DateTime baseDate = new DateTime(2020, 02, 08);
            DateTime holiday = new DateTime(2020, 02, 10);
            DateTime expectedDate = new DateTime(2020, 02, 11);
            
            var calendar = new BusinessCalendar(holiday);
            DateTime actualDate = DateOffsets.Parse("+1D")
                                             .AddWeekdays(1)
                                             .SetCalendar(calendar)
                                             .AddBusinessDays(1)
                                             .ApplyTo(baseDate);
            Assert.AreEqual(expectedDate, actualDate);
        }
    }
}
