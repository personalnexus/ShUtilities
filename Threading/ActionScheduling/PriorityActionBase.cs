using System.Threading;

namespace ShUtilities.Threading.ActionScheduling
{
    /// <summary>
    /// Base class with a partial implementation of <see cref="IPriorityAction"/> the shows thread-safe setting of the scheduler queue.
    /// </summary>
    public class PriorityActionBase
    {
        private object _currentQueue;

        public object SetSchedulerQueue(object newQueue) => Interlocked.Exchange(ref _currentQueue, newQueue);
    }
}
