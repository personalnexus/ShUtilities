using System.Threading;
using ShUtilities.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class TaskTest
    {
        [TestMethod]
        public void RunCancellable()
        {
            CancellationTokenSource cts = null;
            
            Task task1 = TaskUtility.RunCancellable(t => Execute(t, "iteration1"), ref cts);
            CancellationTokenSource cts1 = cts;
            Assert.IsNotNull(cts1);
            Assert.IsFalse(cts1.IsCancellationRequested);
            
            Task task2 = TaskUtility.RunCancellable(t => Execute(t, "iteration2"), ref cts);
            Assert.AreNotEqual(cts, cts1);
            
            Task task3 = TaskUtility.RunCancellable(t => Execute(t, "iteration3"), ref cts);

            Task task4 = TaskUtility.RunCancellable(t => Execute(t, "iteration4"), ref cts);
            task4.Wait();

            Assert.AreEqual(TaskStatus.Canceled, task1.Status);
            Assert.AreEqual(TaskStatus.Canceled, task2.Status);
            Assert.AreEqual(TaskStatus.Canceled, task3.Status);
            Assert.AreEqual(TaskStatus.RanToCompletion, task4.Status);
            Assert.AreEqual("iteration4", _completedIteration);
        }

        private void Execute(CancellationToken ct, string iteration)
        {
            for (int i = 0; i < 5; i++)
            {
                ct.ThrowIfCancellationRequested();
                Thread.Sleep(222);
                if (i == 4 && _completedIteration == null)
                {
                    _completedIteration = iteration;
                }
            }
        }

        private volatile string _completedIteration;

    }
}
