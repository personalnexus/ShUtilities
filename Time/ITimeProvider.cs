using System;

namespace ShUtilities.Time
{
    /// <summary>
    /// An interface to obtain the current time which can be swapped out for tests to simulate progression of time
    /// </summary>
    public interface ITimeProvider
    {
        DateTime Now { get; }

        public long Ticks
        {
            get
            {
                NativeMethods.QueryPerformanceCounter(out long result);
                return result;
            }
        }

        long TicksPerSecond => NativeMethods.TicksPerSecond;
    }
}
