using System;
using System.Threading;

namespace ShUtilities.Threading
{
    /// <summary>
    /// Capture callback and state in a struct instead of the more convenient Action/lambda expression,
    /// because one must "Know Thine Implicit Allocations". See
    /// https://blogs.msdn.microsoft.com/pfxteam/2012/02/03/know-thine-implicit-allocations/
    /// </summary>
    internal struct CallbackAndState
    {
        readonly SendOrPostCallback Callback;
        readonly object State;

        public CallbackAndState(SendOrPostCallback callback, object state)
        {
            Callback = callback;
            State = state;
        }

        internal void Execute()
        {
            Callback(State);
        }
    }
}
