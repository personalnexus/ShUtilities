using System;
using System.Threading;

namespace ShUtilities.Threading.ActionScheduling
{
    internal class PriorityActionSchedulerThread
    {
        private readonly PriorityActionScheduler _scheduler;
        private readonly PriorityActionSchedulerQueue _queue;

        public PriorityActionSchedulerThread(PriorityActionScheduler scheduler, PriorityActionSchedulerQueue queue, int threadIndex)
        {
            _scheduler = scheduler;
            _queue = queue;
            var thread = new Thread(Run)
            { 
                IsBackground = true,
                Name = $"{_scheduler.Options.Name}_P{_queue.Priority}_T{threadIndex}"
            };
            thread.Start();
        }

        private void Run()
        {
            TimeSpan waitTimeout = _scheduler.Options.MaximumWaitDuration;
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
                    if (!SpinWait.SpinUntil(TryDequeue, waitTimeout))
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        _queue.WaitForActions();
                    }
                }
            }
            catch (Exception e)
            {
                if (!(e is OperationCanceledException))
                {
                    _scheduler.OnUnhandledException(new UnhandledExceptionEventArgs(e, false));
                }
            }
        }

        bool TryDequeue()
        {
            bool result = _queue.TryDequeue(out IPriorityAction action);
            if (result)
            {
                action.Execute();
            }
            return result;
        }
    }
}
