using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Collections;
using System.Collections.Generic;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class ConcurrentLookupTest
    {
        [TestMethod]
        public void GetOrEmpty()
        {
            var lookup = new ConcurrentLookup<int, string>();
            lookup.Add(1, "one");
            lookup.Add(2, "two");
            lookup.Add(2, "zwei");

            AssertGetOrEmpty("", 0);
            AssertGetOrEmpty("one", 1);
            AssertGetOrEmpty("two,zwei", 2);

            void AssertGetOrEmpty(string expected, int key) => Assert.AreEqual(expected, lookup.GetOrEmpty(key).ToDelimitedString(","));
        }

        [TestMethod]
        public void Remove()
        {
            var lookup = new ConcurrentLookup<int, string>();
            lookup.Add(1, "one");
            Assert.IsTrue(lookup.TryGetValues(1, out IReadOnlyList<string> ones));
            Assert.AreEqual(1, ones.Count);
            Assert.AreEqual("one", ones[0]);

            lookup.Add(2, "two");
            lookup.Add(2, "zwei");
            Assert.IsTrue(lookup.TryGetValues(2, out IReadOnlyList<string> twos));
            Assert.AreEqual(2, twos.Count);
            Assert.AreEqual("two", twos[0]);
            Assert.AreEqual("zwei", twos[1]);

            Assert.IsFalse(lookup.TryGetValues(0, out IReadOnlyList<string> zeros));
            Assert.IsNull(zeros);

            lookup.Remove(2, "zwei");

            Assert.IsTrue(lookup.TryGetValues(2, out IReadOnlyList<string> oneTwo));
            Assert.AreEqual(1, oneTwo.Count);
            Assert.AreEqual("two", oneTwo[0]);

            lookup.Remove(2, "duo");

            Assert.IsTrue(lookup.TryGetValues(2, out IReadOnlyList<string> stillTwo));
            Assert.AreEqual(1, stillTwo.Count);
            Assert.AreEqual("two", stillTwo[0]);

            lookup.Remove(2, "two");

            Assert.IsFalse(lookup.TryGetValues(2, out IReadOnlyList<string> noTwos));
            Assert.IsNull(noTwos);
        }
    }
}
