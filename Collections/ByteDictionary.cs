using System;
using System.Collections;
using System.Collections.Generic;

namespace ShUtilities.Collections
{
    /// <summary>
    /// Provides <see cref="IDictionary{TKey, TValue}"/>-like access with a fixed memory-footprint to what is essentially an array with a byte as its index
    /// </summary>
    public class ByteDictionary<TValue>: IDictionary<byte, TValue>
    {
        private struct ByteDictionaryItem
        {
            public TValue Value;
            public bool HasValue;
        }

        private ByteDictionaryItem[] _items = new ByteDictionaryItem[byte.MaxValue + 1];

        // IDictionary

        public int Count { get; private set; }

        public bool IsReadOnly { get; set; }

        public ICollection<byte> Keys => throw new NotImplementedException();

        public ICollection<TValue> Values => throw new NotImplementedException();

        public TValue this[byte key]
        {
            get
            {
                if (!TryGetValue(ref _items[key], out TValue value))
                {
                    throw new KeyNotFoundException($"Key {key} does not exist.");
                }
                return value;
            }
            set
            {
                SetValue(ref _items[key], value, true);
            }
        }

        public void Add(KeyValuePair<byte, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Add(byte key, TValue value)
        {
            if (ContainsKey(key))
            {
                throw new ArgumentException($"Key {key} already exists.");
            }
            SetValue(ref _items[key], value, true);
        }

        public void Clear()
        {
            for (int i = 0; i < _items.Length; i++)
            {
                SetValue(ref _items[i], default(TValue), false);
            }
        }

        public bool Contains(KeyValuePair<byte, TValue> item)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(byte key)
        {
            bool result = TryGetValue(ref _items[key], out _);
            return result;
        }

        public bool Remove(byte key)
        {
            bool result = SetValue(ref _items[key], default(TValue), false);
            return result;
        }

        public bool TryGetValue(byte key, out TValue value)
        {
            bool result = TryGetValue(ref _items[key], out value);
            return result;
        }

        public void CopyTo(KeyValuePair<byte, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<byte, TValue> item)
        {
            return Remove(item.Key);
        }

        public IEnumerator<KeyValuePair<byte, TValue>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // Helper methods

        private bool SetValue(ref ByteDictionaryItem byteDictionaryItem, TValue value, bool hasValue)
        {
            if (IsReadOnly)
            {
                throw new InvalidOperationException("Dictionary is read-only.");
            }
            bool hadValue = byteDictionaryItem.HasValue;
            byteDictionaryItem.Value = value;
            byteDictionaryItem.HasValue = hasValue;
            if (!hadValue && hasValue)
            {
                Count++;
            }
            else if (hadValue && !hasValue)
            {
                Count--;
            }
            return hadValue;
        }

        private bool TryGetValue(ref ByteDictionaryItem byteDictionaryItem, out TValue value)
        {
            value = byteDictionaryItem.Value;
            return byteDictionaryItem.HasValue;
        }
    }
}
