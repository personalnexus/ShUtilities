using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using static ShUtilitiesTest.ExceptionUtility;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class CollectionExtensionsTest
    {
        [TestMethod]
        public void ToDelimitedString()
        {
            Assert.AreEqual("1,2,3,4,5", Enumerable.Range(1, 5).ToDelimitedString(","));
        }

        [TestMethod]
        public void ToPartitions()
        {
            var items = new int[] { 1, 2, 3, 4, 5 };
            List<List<int>> partitions = items.ToPartitions(2).ToList();
            Assert.AreEqual(3, partitions.Count);
            // Sized automatically, contains 2
            Assert.AreEqual(2, partitions[0].Count);
            Assert.AreEqual(1, partitions[0][0]);
            Assert.AreEqual(2, partitions[0][1]);
            // Sized for two, contains 2
            Assert.AreEqual(2, partitions[1].Capacity);
            Assert.AreEqual(2, partitions[1].Count);
            Assert.AreEqual(3, partitions[1][0]);
            Assert.AreEqual(4, partitions[1][1]);
            // Sized for two, but only contains 1
            Assert.AreEqual(2, partitions[2].Capacity);
            Assert.AreEqual(1, partitions[2].Count);
            Assert.AreEqual(5, partitions[2][0]);
        }

        [TestMethod]
        public void ToOnePartition()
        {
            var items = new int[] { 1, 2, 3, 4, 5 };
            List<List<int>> partitions = items.ToPartitions(int.MaxValue).ToList();
            Assert.AreEqual(1, partitions.Count);
            Assert.AreNotEqual(int.MaxValue, partitions[0].Capacity);
            CollectionAssert.AreEqual(items, partitions[0]);
        }

        [TestMethod]
        public void TryGetRange()
        {
            var list = new SortedList<int, int>(Enumerable.Range(0, 1000).ToDictionary(x => x, x => x));
            list.Remove(3);

            Check(2, 2, true, 2);
            Check(2, 3, true, 2);
            Check(3, 4, true, 4);
            Check(2, 4, true, 2, 4);
            Check(2, 6, true, 2, 4, 5, 6);
            Check(-1, 2, true, 0, 1, 2);
            Check(999, 1002, true, 999);
            Check(-1, 1002, true, list.Values.ToArray());

            Check(3, 3, false);
            Check(9999, 999999, false);

            Expect<ArgumentException>(() => list.TryGetRange(3, -1, out _));

            void Check(int lowerBound, int upperBound, bool expectedResult, params int[] expectedValues)
            {
                bool actualResult = list.TryGetRange(lowerBound, upperBound, out IEnumerable<int> values);
                Assert.AreEqual(expectedResult, actualResult);
                if (expectedResult)
                {
                    int[] actualValues = values.ToArray();
                    CollectionAssert.AreEqual(expectedValues, actualValues, $"{lowerBound}-{upperBound}: {actualValues.ToDelimitedString(",")}");
                }
            }
        }

        [TestMethod]
        public void IsEmpty()
        {
            Assert.IsTrue(new int[0].IsEmpty());
            Assert.IsFalse(new int[1].IsEmpty());
            Assert.IsTrue(Enumerable.Empty<int>().IsEmpty());
        }

        [TestMethod]
        public void AddNew()
        {
            var list = new List<object>();
            object item = list.AddNew();
            Assert.IsNotNull(item);
            Assert.AreEqual(1, list.Count);
            Assert.AreSame(item, list[0]);
        }

        [TestMethod]
        public void TryFirst()
        {
            var numbers = new[] { 1, 2, 3, 4, 5 };

            Assert.IsTrue(numbers.TryFirst(out int number1));
            Assert.AreEqual(1, number1);

            Assert.IsTrue(numbers.TryFirst(out int number4, x => (3 < x) && (x < 5)));
            Assert.AreEqual(4, number4);

            Assert.IsFalse(numbers.TryFirst(out _, _ => false));

            Assert.IsFalse(new int[0].TryFirst(out _));
        }
    }
}
