using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Diagnostics.Counters;
using System;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class CounterTests
    {
        [TestMethod]
        public void ResetCounter()
        {
            using (var timeProvider = TestTimeProvider.SetDefault())
            {
                var counter = new ResetCounter();
                counter.GetOrAdd(1).Increment();
                counter.GetOrAdd(2).Increment();
                counter.GetOrAdd(1).Increment();

                counter.Update();
                Assert.AreEqual(3, counter.Current);
                Assert.AreEqual(3, counter.Today);
                Assert.AreEqual(3, counter.Total);

                counter.Update();
                Assert.AreEqual(0, counter.Current);
                Assert.AreEqual(3, counter.Today);
                Assert.AreEqual(3, counter.Total);

                counter.Update();
                Assert.AreEqual(0, counter.Current);
                Assert.AreEqual(3, counter.Today);
                Assert.AreEqual(3, counter.Total);

                // New Day
                timeProvider.Now += TimeSpan.FromDays(1);

                counter.Update();
                Assert.AreEqual(0, counter.Current);
                Assert.AreEqual(0, counter.Today);
                Assert.AreEqual(3, counter.Total);

                counter.GetOrAdd(1).Increment();

                counter.Update();
                Assert.AreEqual(1, counter.Current);
                Assert.AreEqual(1, counter.Today);
                Assert.AreEqual(4, counter.Total);

                // New counter value created on the fly
                counter.GetOrAdd(256).Increment();
                counter.GetOrAdd(256).Increment();

                counter.Update();
                Assert.AreEqual(2, counter.Current);
                Assert.AreEqual(3, counter.Today);
                Assert.AreEqual(6, counter.Total);
            }
        }
    }
}
