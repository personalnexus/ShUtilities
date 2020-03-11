using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using ShUtilities.Collections;
using System.Collections.Generic;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class EnumerableExtensionsTest
    {
        [TestMethod]
        public void IsEmpty()
        {
            Assert.IsTrue(new int[0].IsEmpty());
            Assert.IsFalse(new int[1].IsEmpty());
            Assert.IsTrue(Enumerable.Empty<int>().IsEmpty());
        }

        [TestMethod]
        public void SelectWhere()
        {
            var strings = new[] { "1", "2", "bbb", "4", "5K" };
            CollectionAssert.AreEqual(new[] { 1, 2, 4 }, strings.SelectWhere<string, int>(int.TryParse).ToArray());

            var numbers = new[] { 1, 4, 3, 5, 2, 3 };
            var namesById = new Dictionary<int, string>
            {
                [1] = "Jane",
                [2] = "Chris",
                [3] = "Mo",
            };
            CollectionAssert.AreEqual(new[] { "Jane", "Mo", "Chris", "Mo" }, numbers.SelectWhere<int, string>(namesById.TryGetValue).ToArray());

            var discarded = new List<int>();
            numbers.SelectWhere<int, string>(namesById.TryGetValue, discarded).ToArray();
            CollectionAssert.AreEqual(new[] { 4, 5 }, discarded);

        }

        [TestMethod]
        public void ToDelimitedString()
        {
            var mixed = new object[] { null, "", 13, "'" };
            Assert.AreEqual("1,2,3,4,5", Enumerable.Range(1, 5).ToDelimitedString(","));
            Assert.AreEqual(",,13,'", mixed.ToDelimitedString(","));
            Assert.AreEqual("'','','13',''''", mixed.ToDelimitedString(",", "'"));
        }

        [TestMethod]
        public void ToDictionary()
        {
            var items = new int[] { 1, 2, 1 };
            int counter = 1;
            var dictionary1 = items.ToDictionary(x => x, x => counter++, DictionaryDuplicateKey.AcceptFirst);
            CollectionAssert.AreEqual(new[] { 1, 2 }, dictionary1.Values.ToArray(), dictionary1.Values.ToDelimitedString(","));

            counter = 1;
            var dictionary2 = items.ToDictionary(x => x, x => counter++, DictionaryDuplicateKey.AcceptLast);
            CollectionAssert.AreEqual(new[] { 3, 2 }, dictionary2.Values.ToArray(), dictionary2.Values.ToDelimitedString(","));
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
        public void TryFirst()
        {
            var numbers = new[] { 1, 2, 3, 4, 5 };

            Assert.IsTrue(numbers.TryFirst(out int number1));
            Assert.AreEqual(1, number1);

            Assert.IsTrue(numbers.TryFirst(x => (3 < x) && (x < 5), out int number4));
            Assert.AreEqual(4, number4);

            Assert.IsFalse(numbers.TryFirst(_ => false, out _));

            Assert.IsFalse(new int[0].TryFirst(out _));
        }

        [TestMethod]
        public void TryLast()
        {
            var numbers = new[] { 1, 2, 3, 4, 5 };

            Assert.IsTrue(numbers.TryLast(out int number5));
            Assert.AreEqual(5, number5);

            Assert.IsTrue(numbers.TryLast(x => (3 < x) && (x < 5), out int number4));
            Assert.AreEqual(4, number4);

            Assert.IsFalse(numbers.TryLast(_ => false, out _));

            Assert.IsFalse(new int[0].TryLast(out _));
        }
    }
}
