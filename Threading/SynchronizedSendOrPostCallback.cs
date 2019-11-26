using System.Threading;

namespace ShUtilities.Threading
{
    /// <summary>
    /// Synchronizes the execution of a <see cref="SendOrPostCallback"/> that is posted to a <see cref="SynchronizationContext"/>
    /// </summary>
    internal class SynchronizedSendOrPostCallback : SynchronizedExecutionBase
    {
        private readonly SendOrPostCallback _callback;

        internal SynchronizedSendOrPostCallback(SendOrPostCallback callback)
        {
            _callback = callback;
        }

        protected override void RunCore(object state)
        {
            _callback(state);
        }

        internal void GetResult()
        {
            AwaitRun();
        }
    }
}
