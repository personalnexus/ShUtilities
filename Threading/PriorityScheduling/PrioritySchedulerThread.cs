using System;
using System.Threading;

namespace ShUtilities.Threading.PriorityScheduling
{
    internal class PrioritySchedulerThread
    {
        private readonly PriorityScheduler _scheduler;
        private readonly PrioritySchedulerQueue _queue;
        private readonly Thread _thread;

        public PrioritySchedulerThread(PriorityScheduler scheduler, PrioritySchedulerQueue queue, int threadIndex)
        {
            _scheduler = scheduler;
            _queue = queue;
            _thread = new Thread(Run)
            { 
                IsBackground = true,
                Name = $"{queue.Name}_T{threadIndex}"
            };
            _thread.Start();
        }

        private void Run()
        {
            TimeSpan maximumSpinDuration = _scheduler.Options.MaximumSpinDuration;
            TimeSpan maximumWaitDuration = _scheduler.Options.MaximumWaitDuration;
            CancellationToken cancellationToken = _scheduler.CancellationTokenSource.Token;

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    //
                    // Usually, these threads are expected to be pretty busy with long queues. 
                    // If they run out of actions for a moment, spin-wait a little before
                    // entering the lock in WaitForActions()
                    //
                    while (TryDequeue())
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                    }
                    if (!SpinWait.SpinUntil(TryDequeue, maximumSpinDuration))
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        _queue.WaitForActions(maximumWaitDuration);
                    }
                }
            }
            catch (Exception e)
            {
                if (!(e is OperationCanceledException) &&
                    !(e is ThreadAbortException))
                {
                    _scheduler.OnUnhandledException(new UnhandledExceptionEventArgs(e, false));
                }
            }
        }

        bool TryDequeue()
        {
            bool result = _queue.TryDequeue(out CallbackAndState callbackAndState);
            if (result)
            {
                callbackAndState.Execute();
                ExecutedCount++;
            }
            return result;
        }

        // Statistics

        public long ExecutedCount { get; private set; }

        public override string ToString() => $"{_thread.Name} Executed: {ExecutedCount}";
    }
}
