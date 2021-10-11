using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace ShUtilities.Collections
{
    public class ConcurrentLookup<TKey, TValue>
    {
        private readonly Dictionary<TKey, ImmutableArray<TValue>> _valuesByKey = new();
        private readonly ReaderWriterLockSlim _valuesByKeyLock = new();

        public void Add(TKey key, TValue value)
        {
            _valuesByKeyLock.EnterWriteLock();
            try
            {
                if (_valuesByKey.TryGetValue(key, out ImmutableArray<TValue> lookupValues))
                {
                    _valuesByKey[key] = lookupValues.Add(value);
                }
                else
                {
                    _valuesByKey.Add(key, ImmutableArray.Create(value));
                }
            }
            finally
            {
                _valuesByKeyLock.ExitWriteLock();
            }
        }

        public void Remove(TKey key, TValue value)
        {
            _valuesByKeyLock.EnterWriteLock();
            try
            {
                if (_valuesByKey.TryGetValue(key, out ImmutableArray<TValue> lookupValues))
                {
                    ImmutableArray<TValue> newValues = lookupValues.Remove(value);
                    if (newValues.IsEmpty)
                    {
                        _valuesByKey.Remove(key);
                    }
                    else
                    {
                        _valuesByKey[key] = newValues;
                    }
                }
            }
            finally
            {
                _valuesByKeyLock.ExitWriteLock();
            }

        }

        public IEnumerable<TValue> GetOrEmpty(TKey key) => TryGetValues(key, out IReadOnlyList<TValue> values) ? values : Enumerable.Empty<TValue>();

        public bool TryGetValues(TKey key, out IReadOnlyList<TValue> values)
        {
            bool result;
            ImmutableArray<TValue> lookupValues = ImmutableArray<TValue>.Empty;
            _valuesByKeyLock.EnterReadLock();
            try
            {
                result = _valuesByKey.TryGetValue(key, out lookupValues);
            }
            finally
            {
                _valuesByKeyLock.ExitReadLock();
            }
            values = result ? lookupValues : null;
            return result;
        }
    }
}
