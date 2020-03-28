using ShUtilities.Collections;
using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace ShUtilities.Threading.PriorityScheduling
{
    internal class PrioritySchedulerQueue
    {
        private readonly ConcurrentQueue<CallbackAndState> _pendingCallbacks;
        private volatile bool _hasThreadsWaiting;

        private readonly PriorityScheduler _scheduler;
        public string Name { get; }
        public PrioritySchedulerQueue[] OtherQueues { get; set; }
        public IImmutableList<PrioritySchedulerThread> Threads { get; private set; } = ImmutableList.Create<PrioritySchedulerThread>();
        public int ThreadCount
        {
            get => Threads.Count;
            set
            {
                int newThreadCount = value;
                int oldThreadCount = ThreadCount;
                if (newThreadCount < oldThreadCount)
                {
                    Threads.RemoveRange(newThreadCount, oldThreadCount - newThreadCount);
                }
                else if (newThreadCount > oldThreadCount)
                {
                    var newThreads = Enumerable.Range(oldThreadCount, newThreadCount - oldThreadCount).Select(i => new PrioritySchedulerThread(_scheduler, this, i));
                    Threads.AddRange(newThreads);
                }
            }
        }

        public PrioritySchedulerQueue(PriorityScheduler scheduler, int priority, int threadCount)
        {
            Name = $"{scheduler.Options.Name}_P{priority}";
            _scheduler = scheduler;
            _pendingCallbacks = new ConcurrentQueue<CallbackAndState>();
            // Set ThreadCount last so everything is initialized when threads start
            ThreadCount = threadCount;
        }

        public void Enqueue(CallbackAndState callbackAndState)
        {
            _pendingCallbacks.Enqueue(callbackAndState);
            SignalActions();
        }

        public bool TryDequeue(out CallbackAndState callbackAndState)
        {
            bool result = _pendingCallbacks.TryDequeue(out callbackAndState);
            if (!result)
            {
                // Look in the other queues starting from the top (i.e. the top-priority queue)
                for (int i = 0; i < OtherQueues.Length; i++)
                {
                    if (OtherQueues[i].TryLocalDequeue(out callbackAndState))
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        private bool TryLocalDequeue(out CallbackAndState callbackAndState) => _pendingCallbacks.TryDequeue(out callbackAndState);

        private void SignalActions()
        {
            if (_hasThreadsWaiting)
            {
                lock (_pendingCallbacks)
                {
                    if (_hasThreadsWaiting)
                    {
                        _hasThreadsWaiting = false;
                        Monitor.PulseAll(_pendingCallbacks);
                    }
                }
            }
        }

        internal void WaitForActions(TimeSpan maximumWaitDuration)
        {
            lock (_pendingCallbacks)
            {
                _hasThreadsWaiting = true;
                Monitor.Wait(_pendingCallbacks, maximumWaitDuration);
            }
        }

        // Statistics

        public int PendingCount => _pendingCallbacks.Count;
        public long ExecutedCount => Threads.Sum(x => x.ExecutedCount);

        public override string ToString() => $"{Name} Pending: {PendingCount}{Environment.NewLine}{Threads.ToDelimitedString(Environment.NewLine)}";

    }
}
