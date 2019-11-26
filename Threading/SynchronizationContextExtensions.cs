using System;
using System.Threading;

namespace ShUtilities.Threading
{
    /// <summary>
    /// Extension methods for <see cref="SynchronizationContext"/> implementations
    /// </summary>
    public static class SynchronizationContextExtensions
    {
        /// <summary>
        /// Helper for my <see cref="SynchronizationContext"/> implementations that implements Send() as a synchronized version of Post().
        /// </summary>
        internal static void SendHelper(this SynchronizationContext synchronizationContext, SendOrPostCallback callback, object state)
        {
            var execution = new SynchronizedSendOrPostCallback(callback);
            synchronizationContext.Post(execution.Run, state);
            execution.GetResult();
        }

        /// <summary>
        /// Instead of simply sending a callback to the <see cref="SynchronizationContext"/>, send a <see cref="Func{T, TResult}"/> and return its result
        /// </summary>
        public static TResult Send<TResult>(this SynchronizationContext synchronizationContext, Func<object, TResult> func, object state)
        {
            var execution = new SynchronizedFunc<TResult>(func);
            synchronizationContext.Post(execution.Run, state);
            return execution.GetResult();
        }

        /// <summary>
        /// Post the given <see cref="Action"/> to the synchronization context. This allows passing in a lambda expression instead of
        /// an instance of <see cref="SendOrPostCallback"/> which is convenient for closing over local variables instead of having
        /// to wrap them into a state object.
        /// </summary>
        public static void Post(this SynchronizationContext synchronizationContext, Action action)
        {
            synchronizationContext.Post(ExecuteAction, action);
        }

        private static void ExecuteAction(object state)
        {
            var action = (Action)state;
            action();
        }
    }
}
