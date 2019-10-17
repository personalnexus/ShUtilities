using System;

namespace ShUtilities.Time
{
    /// <summary>
    /// An interface to obtain the current time which can be swapped out for tests to simulate progression of time
    /// </summary>
    public interface ITimeProvider
    {
        DateTime Now { get; }
        long Ticks { get; }
    }
}
