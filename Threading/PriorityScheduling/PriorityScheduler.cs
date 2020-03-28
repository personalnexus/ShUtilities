using ShUtilities.Collections;
using System;
using System.Linq;
using System.Threading;

namespace ShUtilities.Threading.PriorityScheduling
{
    /// <summary>
    /// Scheduler for actions with different degrees of concurrency for different priorities.
    /// </summary>
    public class PriorityScheduler: IDisposable
    {
        private readonly PrioritySchedulerQueue[] _queuesByPriority;
        internal CancellationTokenSource CancellationTokenSource { get; }

        public PriorityScheduler(PrioritySchedulerOptions options)
        {
            Options = options;
            Options.Changed += ProcessOptionsChange;
            CancellationTokenSource = new CancellationTokenSource();
            _queuesByPriority = options.ThreadCountsByPriority.Select((priority, threadCount) => new PrioritySchedulerQueue(this, priority, threadCount)).ToArray();
            foreach (PrioritySchedulerQueue queue in _queuesByPriority)
            {
                queue.OtherQueues = _queuesByPriority.Where(x => x != queue).ToArray();
            }
        }

        public void Dispose()
        {
            CancellationTokenSource.Cancel();
        }

        public void Enqueue(SendOrPostCallback callback, object state, int priority) => _queuesByPriority[priority].Enqueue(new CallbackAndState(callback, state));

        public void Enqueue(Action action, int priority) => _queuesByPriority[priority].Enqueue(new CallbackAndState(action));

        public event UnhandledExceptionEventHandler UnhandledException;

        internal void OnUnhandledException(UnhandledExceptionEventArgs eventArgs)
        {
            UnhandledException?.Invoke(this, eventArgs);
            Dispose();
        }

        // Options
        public PrioritySchedulerOptions Options { get; }

        private void ProcessOptionsChange(object sender, PrioritySchedulerOptionsChangedEventArgs e)
        {
            int queueCount = Math.Min(e.NewThreadCountsByPriority.Count, _queuesByPriority.Length);
            for (int priority = 0; priority < queueCount; priority++)
            {
                _queuesByPriority[priority].ThreadCount = e.NewThreadCountsByPriority[priority];
            }
        }

        // Statistics

        public int PendingCount => _queuesByPriority.Sum(x => x.PendingCount);
        public long ExecutedCount => _queuesByPriority.Sum(x => x.ExecutedCount);

        public override string ToString() => _queuesByPriority.ToDelimitedString(Environment.NewLine);
    }
}
