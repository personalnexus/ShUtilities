using System.Threading;

namespace ShUtilities.Threading.ActionScheduling
{
    public class PriorityActionBase
    {
        private object _currentQueue;

        public void SetSchedulerQueue(object newQueue)
        {
            Volatile.Write(ref _currentQueue, newQueue);
        }

        public bool TryExtractSchedulerQueue(object expectedQueue) => Interlocked.CompareExchange(ref _currentQueue, null, expectedQueue) == expectedQueue;
    }
}
