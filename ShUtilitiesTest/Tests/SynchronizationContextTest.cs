using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Text.Markdown;
using ShUtilities.Threading;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class SynchronizationContextTest
    {
        [TestMethod]
        public void SetCurrent()
        {
            var synchronizationContext1 = new SynchronizationContext();
            var synchronizationContext2 = new SynchronizationContext();
            
            Assert.IsNull(SynchronizationContext.Current);
            using (synchronizationContext1.AsCurrent())
            {
                Assert.AreSame(synchronizationContext1, SynchronizationContext.Current);
                using (synchronizationContext2.AsCurrent())
                {
                    Assert.AreSame(synchronizationContext2, SynchronizationContext.Current);
                }
                Assert.AreSame(synchronizationContext1, SynchronizationContext.Current);
            }
            Assert.IsNull(SynchronizationContext.Current);
        }
    }
}
