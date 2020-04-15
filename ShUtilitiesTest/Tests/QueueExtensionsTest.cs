using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ShUtilities.Collections;
using System.Linq;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class QueueExtensionsTest
    {
        [TestMethod]
        public void DequeueMany()
        {
            var queue = new Queue<int>(new [] { 1, 2, 3, 4, 5, 6 });
            int[] oneTwoThree = queue.DequeueMany(3).ToArray();
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, oneTwoThree);
            CollectionAssert.AreEqual(new[] { 4, 5, 6 }, queue);

            int[] fourFive = queue.DequeueMany(2).ToArray();
            CollectionAssert.AreEqual(new[] { 4, 5 }, fourFive);
            CollectionAssert.AreEqual(new[] { 6 }, queue);

            int[] six = queue.DequeueMany(6).ToArray();
            CollectionAssert.AreEqual(new[] { 6 }, six);
            Assert.AreEqual(0, queue.Count);

            int[] none = queue.DequeueMany(256).ToArray();
            Assert.AreEqual(0, none.Length);
            Assert.AreEqual(0, queue.Count);
        }

        [TestMethod]
        public void DequeueManyWithChanges()
        {
            var queue = new Queue<int>(new [] { 1, 2, 3, 4, 5, 6 });
            int dequeuedCount = 0;
            foreach (int i in queue.DequeueMany(9))
            {
                queue.Enqueue(i);
                dequeuedCount++;
            }
            Assert.AreEqual(9, dequeuedCount);
            CollectionAssert.AreEqual(new[] { 4, 5, 6, 1, 2, 3 }, queue);
        }
    }
}
