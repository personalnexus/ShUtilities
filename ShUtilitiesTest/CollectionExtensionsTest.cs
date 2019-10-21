using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ShUtilitiesTest
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
        public void GetOrAdd()
        {
            var dictionary = new Dictionary<string, string>();
            Assert.AreEqual("Value1", dictionary.GetOrAdd("1", CreateValue));
            Assert.AreEqual("Value1", dictionary.GetOrAdd("1", CreateValue));
            Assert.AreEqual("StateX", dictionary.GetOrAdd("X", CreateValue, "State"));
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

        private string CreateValue(string key)
        {
            return "Value" + key;
        }

        private string CreateValue(string key, string state)
        {
            return state + key;
        }
    }
}
