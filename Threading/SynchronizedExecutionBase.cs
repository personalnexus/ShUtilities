using System;
using System.Threading;

namespace ShUtilities.Threading
{
    /// <summary>
    /// Base-class for synchronizing an asynchronous operation posted to a <see cref="SynchronizationContext"/> 
    /// that captures exceptions and re-throws them on the waiting thread after the execution run has completed.
    /// </summary>
    internal abstract class SynchronizedExecutionBase: SetOnceEvent
    {
        private volatile Exception _exception;

        internal void Run(object state)
        {
            try
            {
                RunCore(state);
            }
            catch (Exception exception)
            {
                _exception = exception;
            }
            finally
            {
                Set();
            }
        }

        protected abstract void RunCore(object state);

        protected void AwaitRun()
        {
            Wait();
            Exception exception = _exception;
            if (exception != null)
            {
                throw new AggregateException(exception);
            }
        }
    }
}
