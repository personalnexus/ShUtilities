using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Common;
using ShUtilities.Time;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class TypeUtilityTest
    {
        [TestMethod]
        public void GetEnumValuesByName()
        {
            Dictionary<string, DateOffsetKind> enumValuesByName = TypeUtility.GetEnumValuesByName<DateOffsetKind>();
            Assert.AreEqual(4, enumValuesByName.Count);
            Assert.AreEqual(DateOffsetKind.None, enumValuesByName["None"]);
            Assert.AreEqual(DateOffsetKind.Days, enumValuesByName["Days"]);
            Assert.AreEqual(DateOffsetKind.Weekdays, enumValuesByName["Weekdays"]);
            Assert.AreEqual(DateOffsetKind.BusinessDays, enumValuesByName["BusinessDays"]);
        }
    }
}
