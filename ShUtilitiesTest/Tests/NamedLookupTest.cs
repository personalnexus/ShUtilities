using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Collections;
using System;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class NamedLookupTest
    {
        [TestMethod]
        public void Get()
        {
            var lookup = new NamedLookup("One", "Two", "Three", "Five", "six");
            var one = lookup.Get<int>("One");
            var two = lookup.Get<double>("Two");

            lookup.SetValues(1, 2.0, 3, 5, "SIXSIXSIX");
            Assert.AreEqual(1, one);
            Assert.AreEqual(2.0, two);

            lookup.SetValues(11, 22.2);
            Assert.AreEqual(11, one);
            Assert.AreEqual(22.2, two);
        }

        [TestMethod]
        public void GetValue()
        {
            var lookup = new NamedLookup("One", "Two", "Three", "Five", "six");

            using (lookup.SetValues(1, 2.0, 3, 5, "SIXSIXSIX"))
            {
                Assert.AreEqual(1, lookup.GetValue<int>("One"));
                Assert.AreEqual(2.0, lookup.GetValue<double>("Two"));
            }
            ExceptionUtility.Expect<NullReferenceException>(() => lookup.GetValue<int>("One"));
        }

        [TestMethod]
        public void TryGet()
        {
            var lookup = new NamedLookup("One", "Two", "Three", "Five", "six");
            Assert.IsTrue(lookup.TryGet<bool>("One", out _));
            Assert.IsFalse(lookup.TryGet<int>("None", out _));
        }
    }
}
