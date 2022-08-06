using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Text;
using System;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class StringExtensionsTest
    {
        // 1: Old style

        [TestMethod]
        public void IsNullOrEmpty()
        {
            Assert.IsFalse("not empty".IsNullOrEmpty());
            Assert.IsTrue("".IsNullOrEmpty());
            Assert.IsTrue(((string)null).IsNullOrEmpty());
        }

        // 2: DataTestMethod

        [DataTestMethod]
        [DataRow(new[] { "1", "2" }, DisplayName = "Multiple elements, one matching")]
        [DataRow(new[] { "1", "1" }, DisplayName = "Multiple elements, multiple matching")]
        [DataRow(new[] { "1" }, DisplayName = "Single matching element")]
        public void In_IsTrue(string[] elements) => "1".In(elements).Should().BeTrue();

        [DataTestMethod]
        [DataRow(new string[0], DisplayName = "No elements")]
        [DataRow(new[] { "2", "3" }, DisplayName = "Multiple elements, none matching")]
        [DataRow(new[] { "2" }, DisplayName = "Single non-matching element")]
        public void In_IsFalse(string[] elements) => "1".In(elements).Should().BeFalse();

        // 3: FluentAssertions with AssertionScope for multiple assertions

        [TestMethod]
        public void Left()
        {
            using (new AssertionScope())
            {
                "123456".Left(0).Should().BeEmpty();
                "123456".Left(3).Should().Be("123");
                "123456".Left(6).Should().Be("123456");
                "123456".Invoking(x => x.Left(9)).Should().Throw<ArgumentOutOfRangeException>();
                "123456".Invoking(x => x.Left(-3)).Should().Throw<ArgumentOutOfRangeException>();
            }
        }

        // 4: FluentAssertions with one method per assertion

        [TestMethod]
        public void Right_CountIsZero_Empty() => "123456".Right(0).Should().BeEmpty();

        [TestMethod]
        public void Right_CountIsLessThanLength_Substring() => "123456".Right(3).Should().Be("456");

        [TestMethod]
        public void Right_CountEqualsLength_FullString() => "123456".Right(6).Should().Be("123456");

        [TestMethod]
        public void Right_CountIsMoreThanLength_ArgumentOutOfRangeException() => "123456".Invoking(x => x.Right(9)).Should().Throw<ArgumentOutOfRangeException>();
        
        [TestMethod]
        public void Right_CountIsLessThanZweo_ArgumentOutOfRangeException() => "123456".Invoking(x => x.Right(-3)).Should().Throw<ArgumentOutOfRangeException>();

        // TODO

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

        [TestMethod]
        public void Length()
        {
            Assert.AreEqual(0, ((string)null).Length());
            Assert.AreEqual(0, ("").Length());
            Assert.AreEqual(1, ("0").Length());
            Assert.AreEqual(2, ("-0").Length());
            Assert.AreEqual(8, ("infinity").Length());
            Assert.AreEqual(17, ("negative infinity").Length());
        }
    }
}
