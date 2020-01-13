using System;

namespace ShUtilities.Threading.Pooling
{
    /// <summary>
    /// A ref-struct representing an object from a pool that automatically releases the object when disposed.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public ref struct PooledObject<T>
    {
        private IPool<T> _pool;
        public T Object { get; }

        internal PooledObject(IPool<T> pool)
        {
            Object = pool.Acquire();
            _pool = pool;
        }

        public void Dispose()
        {
            _pool.Release(Object);
        }

        public static implicit operator T(PooledObject<T> pooledObject) => pooledObject.Object;
    }
}
