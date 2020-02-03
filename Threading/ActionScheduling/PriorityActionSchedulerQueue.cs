using System;
using System.Collections.Concurrent;
using System.Threading;

namespace ShUtilities.Threading.ActionScheduling
{
    internal class PriorityActionSchedulerQueue
    {
        private readonly ConcurrentQueue<IPriorityAction> _pendingActions;
        private volatile bool _isWaitingForActions;
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
            SignalActions();
            //TODO: do we need to check if the action was already queued (either in this or in another queue)?
        }

        public bool TryDequeue(out IPriorityAction action)
        {
            bool localDequeueComplete = false;
            do
            {
                //
                // 1. Discard actions that no longer have a queue because 
                //    they will have been processed by a different thread.
                //
                // 2. Accept actions that have a different queue such that
                //    the first thread (even if it has a lower priority) 
                //    will process it.
                //
                if (!_pendingActions.TryDequeue(out action) ||
                    action.SetSchedulerQueue(null) != null)
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

        private void SignalActions()
        {
            if (_isWaitingForActions)
            {
                lock (_pendingActions)
                {
                    if (_isWaitingForActions)
                    {
                        _isWaitingForActions = false;
                        Monitor.PulseAll(_pendingActions);
                    }
                }
            }
        }

        internal void WaitForActions()
        {
            lock (_pendingActions)
            {
                if (_isWaitingForActions)
                {
                    Monitor.Wait(_pendingActions);
                }
            }
        }
    }
}
