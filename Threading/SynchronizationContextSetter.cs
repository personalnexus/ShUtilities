using System;
using System.Threading;

namespace ShUtilities.Threading
{
    /// <summary>
    /// Uses the Dispose pattern to set the current SynchronizationContext to the given one and reset it afterwards to the previous one
    /// </summary>
    public readonly struct SynchronizationContextSetter: IDisposable
    {
        private readonly SynchronizationContext _oldContext;

        public SynchronizationContextSetter(SynchronizationContext newContext)
        {
            _oldContext = SynchronizationContext.Current;
            SynchronizationContext.SetSynchronizationContext(newContext);
        }

        public void Dispose()
        {
            SynchronizationContext.SetSynchronizationContext(_oldContext);
        }
    }
}
