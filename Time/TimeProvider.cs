namespace ShUtilities.Time
{
    /// <summary>
    /// Static class holding a reference to the default time provider (which can be swapped out for unit tests)
    /// </summary>
    public static class TimeProvider
    {
        public static ITimeProvider Default { get; internal set; } = new SystemTimeProvider();
    }
}
