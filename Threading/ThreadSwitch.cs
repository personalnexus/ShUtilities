using ShUtilities.Threading.Awaiters;
using System.Threading;
using System.Threading.Tasks;

namespace ShUtilities.Threading
{
    /// <summary>
    /// Switches threads (ab)using the Awaitable-Awaiter Pattern; inspired by https://devblogs.microsoft.com/oldnewthing/20190328-00/?p=102368
    /// </summary>
    public static class ThreadSwitch
    {
        // WPF
        // public static IAwaiter To(Dispatcher dispatcher) => new DispatcherAwaiter(dispatcher);
        public static IAwaitable To(SynchronizationContext synchronizationContext) => new SynchronizationContextSwitchAwaiter(synchronizationContext);
        public static IAwaitable To(TaskScheduler taskScheduler) => new TaskSchedulerSwitchAwaiter(taskScheduler);
    }
}
