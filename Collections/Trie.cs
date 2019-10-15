using System;
using System.Collections.Generic;

namespace ShUtilities.Collections
{
    public abstract class Trie<TKey, TKeyElement, TValue>
        where TKey: IEnumerable<TKeyElement>
    {
        public Trie(ISet<TKeyElement> possibleKeyElements, int initialCapacity)
        {
            _nodes = new TrieNodes<TKey, TValue>(possibleKeyElements.Count, initialCapacity);
        }

        protected abstract Span<int> TranslateKey(TKey key);

        protected void EnsureMinimumSize<T>(ref T[] array, int minimumSize)
        {
            if (array == null || minimumSize >= array.Length)
            {
                Array.Resize(ref array, minimumSize + 1);
            }
        }

        // IDictionary<TKey, TValue>

        private TrieNodes<TKey, TValue> _nodes;

        public void Add(TKey key, TValue value)
        {
            Span<int> keyIndexes = TranslateKey(key);
            ref TrieNode<TValue> node = ref _nodes.Get(keyIndexes, true);
            node.Value = value;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            Span<int> keyIndexes = TranslateKey(key);
            ref TrieNode<TValue> node = ref _nodes.Get(keyIndexes, false);
            value = node.Value;
            return node.IsAssigned();
        }

        public bool ContainsKey(TKey key)
        {
            bool result = TryGetValue(key, out _);
            return result;
        }

        // TrieInfo

        public TrieInfo GetInfo()
        {
            var result = new TrieInfo
            {
                NodeCount = _nodes.Count,
                IndexSize = _nodes.IndexSize,
            };
            return result;
        }
    }
}
