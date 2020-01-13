using System.Collections.Generic;
using System.Threading;

namespace ShUtilities.Threading.Pooling
{
    /// <summary>
    /// An implementation of <see cref="IPool{T}"/> which provides Thread-safe access to a single pooled object
    /// </summary>
    public struct SingleObjectPool<T>: IPool<T>, IPoolDiagnostics<T>
        where T: class, new()
    {
        private T _object;

        //
        // IPool
        //

        public T Acquire()
        {
            T result = Interlocked.Exchange(ref _object, null);
            if (result == null)
            {
                result = new T();
            }
            return result;
        }

        public void Release(T value)
        {
            Volatile.Write(ref _object, value);
        }

        //
        // IPoolDiagnostics
        //

        public IEnumerable<T> GetObjects()
        {
            yield return _object;
        }

        public int Count => Volatile.Read(ref _object) == null ? 0 : 1;
    }
}
