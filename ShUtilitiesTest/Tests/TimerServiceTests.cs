using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Time;
using System;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class TimerServiceTests
    {
        [TestMethod]
        public void SubscribeInterval()
        {
            using var timeProvider = TestTimeProvider.SetDefault(new DateTime(2020, 03, 15, 17, 0, 0));
            var timerService = new TimerService();

            int intervalsCalled = 0;

            ITimerInterval timerInterval = timerService.SubscribeInterval(TimeSpan.FromHours(1), 
                                                                         (ITimerService sender, ITimerInterval interval) =>
                {
                    Assert.AreEqual(timerService, sender);
                    intervalsCalled++;
                    if (intervalsCalled > 1)
                    {
                        interval.Unsubscribe();
                    }
                });

            timerService.Check();
            Assert.AreEqual(0, intervalsCalled);

            timeProvider.Now += TimeSpan.FromMinutes(31);
            timerService.Check();
            Assert.AreEqual(0, intervalsCalled, "1 hour interval should not have been called after 31 minutes");

            timeProvider.Now += TimeSpan.FromMinutes(31);
            timerService.Check();
            Assert.AreEqual(1, intervalsCalled, "1 hour interval should have been called after 62 minutes");

            timeProvider.Now += TimeSpan.FromMinutes(60);
            timerService.Check();
            Assert.AreEqual(2, intervalsCalled, "should have been called again after 60 minutes");

            timeProvider.Now += TimeSpan.FromMinutes(60);
            timerService.Check();
            Assert.AreEqual(2, intervalsCalled, "should not have been called after unsubscribe");
        }
    }
}
