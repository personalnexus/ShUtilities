using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Common;
using System;
using System.Collections.Generic;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class DisposerTest
    {
        private class TestDisposable : IDisposable
        {
            public void Dispose()
            {
                DisposeCalled++;
            }

            public int DisposeCalled { get; private set; }
        }

        [TestMethod]
        public void Null()
        {
            TestDisposable disposable1 = new TestDisposable();
            TestDisposable disposable2 = disposable1;
            Disposer.Null(ref disposable1);
            Assert.IsNull(disposable1);
            Assert.AreEqual(1, disposable2.DisposeCalled);
        }

        [TestMethod]
        public void All()
        {
            var disposables = new TestDisposable[]
            {
                new TestDisposable(),
                new TestDisposable()
            };
            Disposer.All(disposables);
            foreach (var disposable in disposables)
            {
                Assert.AreEqual(1, disposable.DisposeCalled);
            }
        }

        [TestMethod]
        public void AllNull()
        {
            var disposables1 = new List<TestDisposable>
            {
                new TestDisposable(),
                new TestDisposable()
            };
            List<TestDisposable> disposables2 = disposables1;
            Disposer.AllNull(ref disposables1);
            Assert.IsNull(disposables1);
            foreach (var disposable in disposables2)
            {
                Assert.AreEqual(1, disposable.DisposeCalled);
            }
        }

        [TestMethod]
        public void NullObject()
        {
            var disposable = new TestDisposable();
            disposable = Disposer.Null(disposable);
            Assert.IsNull(disposable);
        }
        
    }
}
