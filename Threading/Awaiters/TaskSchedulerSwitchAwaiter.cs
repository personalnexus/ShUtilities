using System;
using System.Threading.Tasks;

namespace ShUtilities.Threading.Awaiters
{
    internal struct TaskSchedulerSwitchAwaiter : IAwaiter, IAwaitable
    {
        internal TaskSchedulerSwitchAwaiter(TaskScheduler taskScheduler)
        {
            _taskScheduler = taskScheduler;
            _task = null;
        }

        private readonly TaskScheduler _taskScheduler;
        private Task _task;

        public IAwaiter GetAwaiter() => this;

        public bool IsCompleted => _task?.IsCompleted ?? false;

        public void GetResult()
        {
        }

        public void OnCompleted(Action continuation)
        {
            _task = new Task(continuation);
            _task.Start(_taskScheduler);
        }
    }
}
