using System.Collections.Concurrent;

namespace ShUtilities.Threading.ActionScheduling
{
    internal class PriorityActionSchedulerQueue
    {
        private readonly ConcurrentQueue<IPriorityAction> _pendingActions;
        public PriorityActionSchedulerQueue[] OtherQueues { get; set; }
        public int Priority { get; }
        public int Count => _pendingActions.Count;

        public PriorityActionSchedulerQueue(PriorityActionScheduler scheduler, int priority, int threadCount)
        {
            Priority = priority;
            _pendingActions = new ConcurrentQueue<IPriorityAction>();
            for (int i = 0; i < threadCount; i++)
            {
                new PriorityActionSchedulerThread(scheduler, this, i);
            }
        }

        public void Enqueue(IPriorityAction action)
        {
            action.SetSchedulerQueue(this);
            _pendingActions.Enqueue(action);
        }

        public bool TryDequeue(out IPriorityAction action)
        {
            // Discard actions that have a different queue because they will have been procesed by a different thread
            bool localDequeueComplete = false;
            do
            {
                if (!_pendingActions.TryDequeue(out action) ||
                    action.TryExtractSchedulerQueue(this))
                {
                    localDequeueComplete = true;
                }
            }
            while (!localDequeueComplete);

            // Look in the other queues starting from the top (i.e. the top-priority queue)
            if (action == null)
            {
                for (int i = 0; i < OtherQueues.Length; i++)
                {
                    if (OtherQueues[i].TryDequeue(out action))
                    {
                        break;
                    }
                }
            }
            return action != null;
        }
    }
}
