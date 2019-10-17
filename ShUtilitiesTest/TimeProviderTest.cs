using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Time;
using System;

namespace ShUtilitiesTest
{
    [TestClass]
    public class TimeProviderTest
    {
        [TestMethod]
        public void TestTimeProviderAsDefault()
        {
            ITimeProvider previousTimeProvider = TimeProvider.Default;
            Assert.IsInstanceOfType(previousTimeProvider, typeof(SystemTimeProvider));
            var now = new DateTime(2019, 10, 17, 17, 10, 19);
            using (var timeProvider = TestTimeProvider.SetDefault(now))
            {
                Assert.AreSame(timeProvider, TimeProvider.Default);
                Assert.AreEqual(now, TimeProvider.Default.Now);

                timeProvider.Now += TimeSpan.FromSeconds(2);
                Assert.AreEqual(new DateTime(2019, 10, 17, 17, 10, 21), TimeProvider.Default.Now);
            }
            Assert.AreSame(previousTimeProvider, TimeProvider.Default);
        }
    }
}
