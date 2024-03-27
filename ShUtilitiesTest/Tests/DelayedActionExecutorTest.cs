using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Threading.ActionExecutors;
using ShUtilities.Threading.DelayedActions;
using ShUtilities.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class DelayedActionExecutorTest : AssertionScopedTestBase
    {
        [TestClass]
        public class Enqueue
        {
            [TestMethod]
            public void ActionIsNotCancelled_ActionExecutes()
            {
                // Arrange
                Arrange(out TimeSpan delay, out int executedCount, out DelayedActionExecutor executor);

                // Act
                executor.Enqueue(() => executedCount++);
                int pendingCountAfterFirstEnqueue = executor.PendingCount;

                executor.Enqueue(() => executedCount++);
                int pendingCountAfterSecondEnqueue = executor.PendingCount;

                executor.Start();
                Thread.Sleep(2 * delay);
                int pendingCountAfterWait = executor.PendingCount;

                // Assert
                pendingCountAfterFirstEnqueue.Should().Be(1);
                pendingCountAfterSecondEnqueue.Should().Be(2);
                pendingCountAfterWait.Should().Be(0);
                executedCount.Should().Be(2);
                
            }

            [TestMethod]
            public void ActionIsCancelled_ActionDoesNotExecute()
            {
                // Arrange
                Arrange(out TimeSpan delay, out int executedCount, out DelayedActionExecutor executor);

                // Act
                ICancelable firstAction = executor.Enqueue(() => executedCount++);
                ICancelable secondAction = executor.Enqueue(() => executedCount++);

                executor.Start();
                Thread.Sleep(delay / 2);

                bool firstActionWasCancelled = firstAction.TryCancel();
                bool secondActionWasCancelled = secondAction.TryCancel();

                Thread.Sleep(2 * delay);
                int pendingCountAfterWait = executor.PendingCount;

                // Assert
                pendingCountAfterWait.Should().Be(0);
                executedCount.Should().Be(0);
                firstActionWasCancelled.Should().BeTrue();
                secondActionWasCancelled.Should().BeTrue();
            }

            private static void Arrange(out TimeSpan delay, out int executedCount, out DelayedActionExecutor executor)
            {
                delay = TimeSpan.FromSeconds(2);
                executedCount = 0;
                var timeProvider = new SystemTimeProvider();
                executor = new DelayedActionExecutor(timeProvider,
                                                     TaskActionExecutor.Default,
                                                     delay);
            }
        }
    }
}
