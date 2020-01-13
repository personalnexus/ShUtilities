namespace ShUtilities.Threading.Pooling
{
    /// <summary>
    /// Describes a pool from which objects can be acquired and released
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPool<T>
    {
        T Acquire();
        void Release(T value);
    }
}
