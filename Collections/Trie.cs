using System;
using System.Collections.Generic;
using System.Linq;

namespace ShUtilities.Collections
{
    public class Trie<TKey, TKeyElement, TValue>
        where TKey: IEnumerable<TKeyElement>
    {
        private const int MaxKeyCount = byte.MaxValue + 1;

        public Trie(ISet<TKeyElement> possibleKeyElements, int initialCapacity)
        {
            if (possibleKeyElements.Count > MaxKeyCount)
            {
                throw new ArgumentOutOfRangeException($"Trie only supports up to {MaxKeyCount} distinct key elements, not {possibleKeyElements.Count}.");
            }
            
            _keyIndexByKeyElement = new Dictionary<TKeyElement, byte>(possibleKeyElements.Count);
            _keyElementsByIndex = new TKeyElement[possibleKeyElements.Count+1];
            byte index = 1;
            foreach (TKeyElement keyElement in possibleKeyElements)
            {
                _keyIndexByKeyElement[keyElement] = index;
                _keyElementsByIndex[index] = keyElement;
                index++;
            }

            _nodes = new TrieNodes<TKey, TValue>((byte)possibleKeyElements.Count, initialCapacity);
        }

        // Key elements are translated into an array of byte-sized indexes to speed up search and limit the space of the nodes array

        private readonly Dictionary<TKeyElement, byte> _keyIndexByKeyElement;
        private readonly TKeyElement[] _keyElementsByIndex;

        public TrieKey CreateKey(TKey key)
        {
            TKeyElement[] keyElements = key.ToArray();
            TrieKey result = CreateKey(keyElements, keyElements.Length);
            return result;
        }

        public TrieKey CreateKey(IEnumerable<TKeyElement> keyElements, int length)
        {
            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException($"Length of key must be greater than zero.");
            }
            var indexes = new byte[length];
            byte i = 0;
            foreach (TKeyElement keyElement in keyElements)
            {
                indexes[i] = _keyIndexByKeyElement[keyElement];
                i++;
            }
            var result = new TrieKey(indexes);
            return result;
        }

        public IEnumerable<TKeyElement> GetKeyElements(TrieKey key)
        {
            // If one passes a TrieKey that was not created by this Trie, the array access
            // _keyElementsByIndex[i] might fail with an out-of-range exception. And even
            // if by chance it doesn't, the result will be garbage
            IEnumerable<TKeyElement> result = key.Indexes.Select(i => _keyElementsByIndex[i]);
            return result;
        }

        // IDictionary<TKey, TValue>

        private TrieNodes<TKey, TValue> _nodes;

        public TrieKey Add(TKey key, TValue value)
        {
            TrieKey result = CreateKey(key);
            Add(result, value);
            return result;
        }

        public void Add(TrieKey key, TValue value)
        {
            ref TrieNode<TValue> node = ref _nodes.Get(key.Indexes, true);
            node.Value = value;
        }

        public bool TryGetValue(TrieKey key, out TValue value)
        {
            ref TrieNode<TValue> node = ref _nodes.Get(key.Indexes, false);
            value = node.Value;
            return node.IsAssigned();
        }

        public bool ContainsKey(TrieKey key)
        {
            bool result = TryGetValue(key, out _);
            return result;
        }

        public bool ContainsKey(TKey key)
        {
            TrieKey trieKey = CreateKey(key);
            bool result = ContainsKey(trieKey);
            return result;
        }

        // TrieInfo

        public TrieInfo GetInfo()
        {
            var result = new TrieInfo
            {
                NodeCount = _nodes.Count
            };
            return result;
        }
    }
}
