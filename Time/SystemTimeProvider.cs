using System;

namespace ShUtilities.Time
{
    /// <summary>
    /// An implementation of ITimeProvider that provides the time from DateTime.Now and ticks via QueryPerformanceCounter
    /// </summary>
    public class SystemTimeProvider : ITimeProvider
    {
        public DateTime Now => DateTime.Now;

        public long Ticks
        {
            get
            {
                NativeMethods.QueryPerformanceCounter(out long result);
                return result;
            }
        }
    }
}
