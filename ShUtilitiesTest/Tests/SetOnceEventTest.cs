using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Threading;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class SetOnceEventTest
    {
        [TestMethod]
        public void SetAndWait()
        {
            var waitEvent = new SetOnceEvent();
            Assert.IsFalse(waitEvent.IsSet);
            string value = "not signaled";
            Task waiter = Task.Run(() =>
            {
                waitEvent.Wait();
                Console.WriteLine(value);
            });
            Task setter = Task.Run(() =>
            {
                Thread.Sleep(2222);
                value = "signaled";
                waitEvent.Set();
            });
            Task.WaitAll(waiter, setter);
            Assert.IsTrue(waitEvent.IsSet);
            Assert.AreEqual("signaled", value);
        }
    }
}
