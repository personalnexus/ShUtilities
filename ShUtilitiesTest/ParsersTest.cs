using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Text;
using System;

namespace ShUtilitiesTest
{
    [TestClass]
    public class ParsersTest
    {
        [TestMethod]
        public void ParseShort()
        {
            var parsers = Parsers.CreateDefault();
            Assert.AreEqual((short)123, parsers.Parse<short>("123"));
        }

        [TestMethod]
        public void ParseString()
        {
            var parsers = Parsers.CreateDefault();
            Assert.IsNull(parsers.Parse<string>(null));
            Assert.AreEqual("", parsers.Parse<string>(""));
            Assert.AreEqual("Hello", parsers.Parse<string>("Hello"));
        }

        [TestMethod]
        public void ParseNullable()
        {
            var parsers = Parsers.CreateDefault();
            Assert.IsNull(parsers.Parse<short?>(""));
            Assert.IsNull(parsers.Parse<DateTime?>(null));
        }
    }
}
