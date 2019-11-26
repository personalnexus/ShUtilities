using System;
using System.Threading;

namespace ShUtilities.Threading
{
    /// <summary>
    /// A light-weight alternative to ManualResetEvent for events that are only set once that does not need to be disposed
    /// </summary>
    public class SetOnceEvent
    {
        /// <summary>
        /// Use a private object for locking, so no outside code can interfere with the event
        /// </summary>
        private readonly object _lockObject = new object();

        private volatile bool _isSet;

        public bool IsSet => _isSet;

        public void Set()
        {
            if (!_isSet)
            {
                lock (_lockObject)
                {
                    _isSet = true;
                    Monitor.PulseAll(_lockObject);
                }
            }
        }

        public bool Wait()
        {
            return Wait(Timeout.Infinite);
        }

        public bool Wait(TimeSpan timeout)
        {
            return Wait((int)timeout.TotalMilliseconds);
        }

        public bool Wait(int timeoutInMilliseconds)
        {
            bool result;
            if (_isSet)
            {
                result = true;
            }
            else
            {
                lock (_lockObject)
                {
                    // Don't think there is a risk of spurious wake-ups since _isSet can
                    // never become false if it was true once, but just in case we check
                    // it again
                    result = Monitor.Wait(_lockObject, timeoutInMilliseconds) && _isSet;
                }
            }
            return result;
        }
    }
}
