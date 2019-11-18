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
    }
}
