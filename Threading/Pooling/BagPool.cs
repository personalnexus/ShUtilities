using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ShUtilities.Threading.Pooling
{
    /// <summary>
    /// An implementation of <see cref="IPool{T}"/> that uses a <see cref="ConcurrentBag{T}"/> as the underlying storage.
    /// </summary>
    public class BagPool<T> : IPool<T>, IPoolDiagnostics<T>
        where T: new()
    {
        private readonly ConcurrentBag<T> _objects = new ConcurrentBag<T>();

        //
        // IPool
        //

        public T Acquire()
        {
            if (!_objects.TryTake(out T result))
            {
                result = new T();
            }
            return result;
        }

        public void Release(T obj)
        {
            _objects.Add(obj);
        }

        //
        // IPoolDiagnostics
        //

        public int Count => _objects.Count;

        public IEnumerable<T> GetObjects() => _objects;

    }
}
