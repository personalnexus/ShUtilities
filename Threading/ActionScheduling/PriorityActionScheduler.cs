using System;
using System.Linq;
using System.Threading;

namespace ShUtilities.Threading.ActionScheduling
{
    /// <summary>
    /// Scheduler for actions with different degrees of concurrency for different priorities. 
    /// </summary>
    public class PriorityActionScheduler: IDisposable
    {
        private readonly PriorityActionSchedulerQueue[] _queuesByPriority;
        internal CancellationTokenSource CancellationTokenSource { get; }

        public PriorityActionSchedulerOptions Options { get; }

        public int Count => _queuesByPriority.Sum(x => x.Count);

        public PriorityActionScheduler(PriorityActionSchedulerOptions options)
        {
            Options = options;
            CancellationTokenSource = new CancellationTokenSource();
            _queuesByPriority = options.ThreadCountsByPriority.Select((priority, threadCount) => new PriorityActionSchedulerQueue(this, priority, threadCount)).ToArray();
            foreach (PriorityActionSchedulerQueue queue in _queuesByPriority)
            {
                queue.OtherQueues = _queuesByPriority.Where(x => x != queue).ToArray();
            }
        }

        public void Dispose()
        {
            CancellationTokenSource.Cancel();
        }

        public void Enqueue(IPriorityAction action, int priority)
        {
            _queuesByPriority[priority].Enqueue(action);
        }

        public event UnhandledExceptionEventHandler UnhandledException;

        internal void OnUnhandledException(UnhandledExceptionEventArgs eventArgs)
        {
            UnhandledException?.Invoke(this, eventArgs);
            Dispose();
        }
    }
}
