using System;
using System.Threading;

namespace ShUtilities.Threading.Awaiters
{
    internal struct SynchronizationContextSwitchAwaiter : IAwaiter, IAwaitable
    {
        internal SynchronizationContextSwitchAwaiter(SynchronizationContext synchronizationContext)
        {
            _synchronizationContext = synchronizationContext;
        }

        private readonly SynchronizationContext _synchronizationContext;

        public IAwaiter GetAwaiter() => this;

        public bool IsCompleted => SynchronizationContext.Current == _synchronizationContext;

        public void GetResult()
        {
        }

        public void OnCompleted(Action continuation)
        {
            _synchronizationContext.Post(continuation);
        }
    }
}