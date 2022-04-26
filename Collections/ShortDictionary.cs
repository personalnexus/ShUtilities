using System;
using System.Collections;
using System.Collections.Generic;

namespace ShUtilities.Collections
{
    /// <summary>
    /// Provides <see cref="IDictionary{TKey, TValue}"/>-like access with a fixed memory-footprint to what is essentially an array with a short (cast to ushort) as its index
    /// </summary>
    public class ShortDictionary<TValue> : IDictionary<short, TValue>
    {
        private struct ShortDictionaryItem
        {
            public TValue Value;
            public bool HasValue;
        }

        private readonly ShortDictionaryItem[] _items = new ShortDictionaryItem[ushort.MaxValue + 1];

        // IDictionary

        public int Count { get; private set; }

        public bool IsReadOnly { get; set; }

        public ICollection<short> Keys => throw new NotImplementedException();

        public ICollection<TValue> Values => throw new NotImplementedException();

        public TValue this[short key]
        {
            get
            {
                if (!TryGetValue(key, out TValue value))
                {
                    throw new KeyNotFoundException($"Key {key} does not exist.");
                }
                return value;
            }
            set
            {
                SetValue(key, true, value);
            }
        }

        public void Add(KeyValuePair<short, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Add(short key, TValue value)
        {
            if (ContainsKey(key))
            {
                throw new ArgumentException($"Key {key} already exists.");
            }
            SetValue(key, true, value);
        }

        public void Clear()
        {
            CheckReadOnly();
            Array.Clear(_items, 0, _items.Length);
            Count = 0;
        }

        public bool Contains(KeyValuePair<short, TValue> item)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(short key)
        {
            ref ShortDictionaryItem item = ref GetItem(key);
            return item.HasValue;
        }

        public bool Remove(short key)
        {
            bool result = SetValue(key, false, default);
            return result;
        }

        public bool TryGetValue(short key, out TValue value)
        {
            ref ShortDictionaryItem item = ref GetItem(key);
            value = item.Value;
            return item.HasValue;
        }

        public void CopyTo(KeyValuePair<short, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<short, TValue> item)
        {
            return Remove(item.Key);
        }

        public IEnumerator<KeyValuePair<short, TValue>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // Helper methods

        private bool SetValue(short key, bool hasValue, TValue value)
        {
            CheckReadOnly();
            ref ShortDictionaryItem item = ref GetItem(key);
            bool hadValue = item.HasValue;
            item.Value = value;
            item.HasValue = hasValue;
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

        private void CheckReadOnly()
        {
            if (IsReadOnly)
            {
                throw new InvalidOperationException("Dictionary is read-only.");
            }
        }

        private ref ShortDictionaryItem GetItem(short key)
        {
            return ref _items[unchecked((uint)key)];
        }
    }
}
