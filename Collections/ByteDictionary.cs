﻿using System;
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
            SetValue(key, true, value);
        }

        public void Clear()
        {
            Array.Clear(_items, 0, _items.Length);
            Count = 0;
        }

        public bool Contains(KeyValuePair<byte, TValue> item)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(byte key)
        {
            bool result = TryGetValue(key, out _);
            return result;
        }

        public bool Remove(byte key)
        {
            bool result = SetValue(key, false, default);
            return result;
        }

        public bool TryGetValue(byte key, out TValue value)
        {
            ref ByteDictionaryItem byteDictionaryItem = ref GetItem(key);
            value = byteDictionaryItem.Value;
            return byteDictionaryItem.HasValue;
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

        private bool SetValue(byte key, bool hasValue, TValue value)
        {
            if (IsReadOnly)
            {
                throw new InvalidOperationException("Dictionary is read-only.");
            }
            ref ByteDictionaryItem byteDictionaryItem = ref GetItem(key);
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

        private ref ByteDictionaryItem GetItem(byte key)
        {
            return ref _items[key];
        }
    }
}
