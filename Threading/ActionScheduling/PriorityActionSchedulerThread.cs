using System;
using System.Threading;

namespace ShUtilities.Threading.ActionScheduling
{
    internal class PriorityActionSchedulerThread
    {
        private readonly PriorityActionScheduler _scheduler;
        private readonly PriorityActionSchedulerQueue _queue;
        private readonly CancellationToken _cancellationToken;

        public PriorityActionSchedulerThread(PriorityActionScheduler scheduler, PriorityActionSchedulerQueue queue, int threadIndex)
        {
            _cancellationToken = scheduler.CancellationTokenSource.Token;
            _scheduler = scheduler;
            _queue = queue;
            var thread = new Thread(Run)
            { 
                IsBackground = true,
                Name = $"{_scheduler.Options.Name}_Q{_queue.Priority}_T{threadIndex}"
            };
            thread.Start();
        }

        private void Run()
        {
            int wait3Milliseconds = (int)_scheduler.Options.MaximumWaitDuration.TotalMilliseconds;
            int wait2Milliseconds = wait3Milliseconds / 2;
            int wait1Milliseconds = wait2Milliseconds / 4;

            while (true)
            {
                try
                {
                    TryDequeueAndWait(wait1Milliseconds);
                    TryDequeueAndWait(wait2Milliseconds);
                    TryDequeueAndWait(wait3Milliseconds);
                }
                catch (Exception e)
                {
                    if (!(e is OperationCanceledException))
                    {
                        _scheduler.OnUnhandledException(new UnhandledExceptionEventArgs(e, false));
                    }
                    break;
                }
            }
            
            void TryDequeueAndWait(int waitMilliseconds)
            {
                while (_queue.TryDequeue(out IPriorityAction action))
                {
                    _cancellationToken.ThrowIfCancellationRequested();
                    action.Execute();
                }
                _cancellationToken.ThrowIfCancellationRequested();
                Thread.Sleep(waitMilliseconds);
            }
        }
    }
}
