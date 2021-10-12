using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Threading;
using System.Collections.Generic;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class ReadWriteLockerTest
    {
        [TestMethod]
        public void CreateRead()
        {
            var list = new List<int> { 2, 4, 8 };
            var locker = ReadWriteLocker.Create(list);
            using (locker.Read())
            {
                Assert.IsFalse(locker.Lock.IsWriteLockHeld);
                Assert.IsTrue(locker.Lock.IsReadLockHeld);
                CollectionAssert.AreEqual(new[] { 2, 4, 8 }, locker.Value);
            }
            Assert.IsFalse(locker.Lock.IsWriteLockHeld);
            Assert.IsFalse(locker.Lock.IsReadLockHeld);
        }

        [TestMethod]
        public void CreateNewWrite()
        {
            var locker = ReadWriteLocker.Create<List<int>>();
            using (locker.Write())
            {
                FillList(locker); // ReadWriteLocker supports implicit cast into the wrapped type
                Assert.IsTrue(locker.Lock.IsWriteLockHeld);
                Assert.IsFalse(locker.Lock.IsReadLockHeld);
            }
            Assert.IsFalse(locker.Lock.IsWriteLockHeld);
            Assert.IsFalse(locker.Lock.IsReadLockHeld);
            Assert.AreEqual(3, locker.Value.Count);
        }

        private void FillList(List<int> list)
        {
            list.Add(0);
            list.Add(1);
            list.Add(2);
        }
    }
}
