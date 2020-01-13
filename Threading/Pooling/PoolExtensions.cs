namespace ShUtilities.Threading.Pooling
{
    /// <summary>
    /// Extension methods for <see cref="IPool{T}"/>
    /// </summary>
    public static class PoolExtensions
    {
        public static PooledObject<T> AcquireDisposable<T>(this IPool<T> pool) => new PooledObject<T>(pool);
    }
}
