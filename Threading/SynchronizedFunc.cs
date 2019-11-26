using System;
using System.Threading;

namespace ShUtilities.Threading
{
    /// <summary>
    /// Synchronizes the execution of a <see cref="Func{T, TResult}"/> that is posted to a <see cref="SynchronizationContext"/>
    /// </summary>
    internal class SynchronizedFunc<TResult>: SynchronizedExecutionBase
    {
        private readonly Func<object, TResult> _func;
        private TResult _result;

        internal SynchronizedFunc(Func<object, TResult> func)
        {
            _func = func;
        }

        protected override void RunCore(object state)
        {
            _result = _func(state);
        }

        internal TResult GetResult()
        {
            AwaitRun();
            return _result;
        }
    }
}
