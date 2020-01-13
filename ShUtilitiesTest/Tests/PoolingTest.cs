using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Threading.Pooling;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class PoolingTest
    {
        [TestMethod]
        public void SingleObjectPooling()
        {
            var pool = new SingleObjectPool<ArrayList>();

            ArrayList arrayList1 = pool.Acquire();
            CheckArrayListIsNew(arrayList1);
            pool.Release(arrayList1);

            // After the object is released, it can be re-acquired
            ArrayList arrayList2 = pool.Acquire();
            CheckArrayListIsNew(arrayList2);
            Assert.AreSame(arrayList1, arrayList2);
            
            // Before the object is released, the next Acquire() call will create a new object
            ArrayList arrayList3 = pool.Acquire();
            CheckArrayListIsNew(arrayList2);
            Assert.AreNotSame(arrayList2, arrayList3);
        }

        [TestMethod]
        public void AcquireDisposable()
        {
            BagPool<ArrayList> pool = new BagPool<ArrayList>();

            using var arrayList1 = pool.AcquireDisposable();
            CheckArrayListIsNew(arrayList1);

            using var arrayList2 = pool.AcquireDisposable();
            CheckArrayListIsNew(arrayList2);
            
            Assert.AreNotSame(arrayList1.Object, arrayList2.Object);
        }

        private void CheckArrayListIsNew(ArrayList arrayList)
        {
            Assert.IsNotNull(arrayList);
            Assert.AreEqual(0, arrayList.Count);
        }
    }
}
