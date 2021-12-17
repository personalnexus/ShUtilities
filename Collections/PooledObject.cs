using Microsoft.Extensions.ObjectPool;

namespace ShUtilities.Collections
{
    public static class ObjectPoolExtensions
    {
        public static PooledObject<T> Get<T>(this ObjectPool<T> pool, out T value)
            where T : class 
        {
            value = pool.Get();
            return new PooledObject<T>(pool, value);
        }
    }
    
    public readonly ref struct PooledObject<T>
        where T: class
    {
        private readonly ObjectPool<T> _pool;
        private readonly T _value;

        public PooledObject(ObjectPool<T> pool, T value)
        {
            _pool = pool;
            _value = value;
        }

        public void Dispose() => _pool.Return(_value);
    }
}
