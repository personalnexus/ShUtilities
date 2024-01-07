using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Time;
using System;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class TimeProviderTest
    {
        [TestMethod]
        public void TestTimeProviderAsDefault()
        {
            ITimeProvider previousTimeProvider = ShUtilities.Time.TimeProvider.Default;
            Assert.IsInstanceOfType(previousTimeProvider, typeof(SystemTimeProvider));
            var now = new DateTime(2019, 10, 17, 17, 10, 19);
            using (var timeProvider = TestTimeProvider.SetDefault(now))
            {
                Assert.AreSame(timeProvider, ShUtilities.Time.TimeProvider.Default);
                Assert.AreEqual(now, ShUtilities.Time.TimeProvider.Default.Now);

                timeProvider.Now += TimeSpan.FromSeconds(2);
                Assert.AreEqual(new DateTime(2019, 10, 17, 17, 10, 21), ShUtilities.Time.TimeProvider.Default.Now);
            }
            Assert.AreSame(previousTimeProvider, ShUtilities.Time.TimeProvider.Default);
        }
    }
}
