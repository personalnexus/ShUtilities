using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ShUtilities.Threading.Pooling
{
    /// <summary>
    /// An implementation of <see cref="IPool{T}"/> that uses a <see cref="ConcurrentQueue{T}"/> as the underlying storage.
    /// </summary>
    public class QueuePool<T> : IPool<T>, IPoolDiagnostics<T>
        where T : new()
    {
        private ConcurrentQueue<T> _objects = new ConcurrentQueue<T>();

        public T Acquire()
        {
            if (!_objects.TryDequeue(out T result))
            {
                result = new T();
            }
            return result;
        }

        public void Release(T obj)
        {
            _objects.Enqueue(obj);
        }

        //
        // IPoolDiagnostics
        //

        public int Count => _objects.Count;

        public IEnumerable<T> GetObjects() => _objects;
    }
}
