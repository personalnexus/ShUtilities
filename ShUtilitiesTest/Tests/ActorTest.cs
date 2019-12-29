using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Threading;
using ShUtilities.Collections;
using System.Threading;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class ActorTest
    {
        [TestMethod]
        public void ExecuteAsync()
        {
            var stock = new Stock();
            for (int i = 1; i <= 1000; i++)
            {
                stock.UpdatePriceAsync(i);
                if (i % 100 == 0)
                {
                    Thread.Sleep(200);
                }
            }
            Assert.IsTrue(stock.PriceJumps.Count >= 10);
            Console.Write(stock.PriceJumps.ToDelimitedString(Environment.NewLine));
        }

        [TestMethod]
        public async Task ThreadSwitching()
        {
            var stock = new Stock();
            Assert.IsNull(SynchronizationContext.Current);
            await ThreadSwitch.To(stock);
            Assert.AreEqual(stock, SynchronizationContext.Current);
        }

        private class Stock : ActorSynchronizationContext
        {
            public Stock() : base(TaskScheduler.Default)
            {
                PriceJumps = new List<int>();
            }

            private int _oldPrice;
            public int CurrentPrice { get; private set; }
            public List<int> PriceJumps;

            public void UpdatePriceAsync(int newPrice)
            {
                this.Post(() => CurrentPrice = newPrice);
            }

            public override void OperationStarted()
            {
                base.OperationStarted();
                _oldPrice = CurrentPrice;
            }

            public override void OperationCompleted()
            {
                base.OperationCompleted();
                PriceJumps.Add(CurrentPrice - _oldPrice);
            }
        }
    }
}
