using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Text;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class StringExtensionsTest
    {
        [TestMethod]
        public void IsNullOrEmpty()
        {
            Assert.IsFalse("not empty".IsNullOrEmpty());
            Assert.IsTrue("".IsNullOrEmpty());
            Assert.IsTrue(((string)null).IsNullOrEmpty());
        }

        [TestMethod]
        public void In()
        {
            Assert.IsTrue("1".In("1", "2"));
            Assert.IsTrue("1".In("1"));
            Assert.IsFalse("1".In());
            Assert.IsFalse("1".In(""));
            Assert.IsFalse("1".In("2"));
        }

        [TestMethod]
        public void Left()
        {
            Assert.AreEqual("123", "123456".Left(3));
            Assert.AreEqual("", "123456".Left(0));
        }

        [TestMethod]
        public void Right()
        {
            Assert.AreEqual("456", "123456".Right(3));
            Assert.AreEqual("", "123456".Right(0));
        }

        [TestMethod]
        public void Between()
        {
            Assert.AreEqual("Between", "LeftBetweenRight".Between("Left", "Right"));
            Assert.AreEqual("Between", "BetweenRight".Between("Left", "Right"));
            Assert.AreEqual("Between", "LeftBetween".Between("Left", "Right"));
            Assert.AreEqual("", "LeftRight".Between("Left", "Right"));
            Assert.IsNull("LBetweenR".Between("Left", "Right"));
            Assert.IsNull("".Between("Left", "Right"));
        }
    }
}
