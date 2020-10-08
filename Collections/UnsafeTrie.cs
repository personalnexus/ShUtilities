using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ShUtilities.Collections
{
    /// <summary>
    /// An experimental copy&paste version of <see cref="Trie{TValue}"/>that uses the <see cref="Unsafe"/> class for slightly faster array access
    /// </summary>
    public class UnsafeTrie<TValue>: IDictionary<string, TValue>
    {
        public UnsafeTrie(ISet<char> possibleCharacters, int initialCapacity, int capacityIncrement)
        {
            int index = 1;
            foreach (char character in possibleCharacters)
            {
                if (_keyIndexByCharacter == null || character >= _keyIndexByCharacter.Length)
                {
                    Array.Resize(ref _keyIndexByCharacter, character + 1);
                }
                _keyIndexByCharacter[character] = index;
                index++;
            }
            _capacityIncrement = capacityIncrement;
            _possibleCharacterCount = possibleCharacters.Count;
            Resize(initialCapacity);

        }

        // Key lookup

        private readonly int[] _keyIndexByCharacter;

        // Nodes

        private TrieNode<TValue>[] _nodes;
        private int[] _nodeIndexes;
        private int _lastUsedNodeIndex;

        private readonly int _possibleCharacterCount;
        private readonly int _capacityIncrement;

        private ref TrieNode<TValue> GetNode(string key, bool createIfMissing)
        {
            int nodeIndex = 0;
            ref int firstKeyIndex = ref _keyIndexByCharacter[0];
            ref int firstNodeIndex = ref _nodeIndexes[0];
            foreach (char character in key)
            {
                // Step 1: get the index of where in the _indexes array the index into _nodes is found
                int indexIndex = (nodeIndex * _possibleCharacterCount) + Unsafe.Add(ref firstKeyIndex, character);
                // Step 2: get the index of the value in _values
                nodeIndex = Unsafe.Add(ref firstNodeIndex, indexIndex);
                if (nodeIndex == 0)
                {
                    if (createIfMissing)
                    {
                        _lastUsedNodeIndex++;
                        if (_lastUsedNodeIndex == _nodes.Length)
                        {
                            // Resize() changes _nodexIndexes, so the reference in firstNodeIndex becomes invalid and we have to re-initialize
                            Resize(_nodes.Length + _capacityIncrement);
                            firstNodeIndex = ref _nodeIndexes[0];
                        }
                        nodeIndex = _nodeIndexes[indexIndex] = _lastUsedNodeIndex;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return ref _nodes[nodeIndex];
        }

        private void Resize(int newSize)
        {
            Array.Resize(ref _nodes, newSize);
            Array.Resize(ref _nodeIndexes, newSize * _possibleCharacterCount);
        }

        // IDictionary<string, TValue>

        public ICollection<string> Keys => throw new NotImplementedException();

        public ICollection<TValue> Values => throw new NotImplementedException();

        public int Count { get; private set; }

        public bool IsReadOnly { get; set; }

        public TValue this[string key]
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
                SetValue(key, value, false);
            }
        }

        public void Add(KeyValuePair<string, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Add(string key, TValue value)
        {
            SetValue(key, value, true);
        }

        private void SetValue(string key, TValue value, bool throwIfKeyExists)
        {
            ref TrieNode<TValue> node = ref GetNode(key, true);
            if (!node.HasValue)
            {
                Count++;
            }
            else if (throwIfKeyExists)
            {
                throw new ArgumentException($"Key {key} already exists.");
            }
            node.Value = value;
            node.HasValue = true;
        }

        public void Clear()
        {
            CheckReadOnly();
            _nodes = new TrieNode<TValue>[1];
            _nodeIndexes = new int[_possibleCharacterCount];
            _lastUsedNodeIndex = 0;
            Count = 0;
        }

        public bool ContainsKey(string key)
        {
            ref TrieNode<TValue> node = ref GetNode(key, false);
            return node.HasValue;
        }

        public bool Contains(KeyValuePair<string, TValue> item)
        {
            return ContainsKey(item.Key);
        }

        public void CopyTo(KeyValuePair<string, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<string, TValue> item)
        {
            return Remove(item.Key);
        }

        public bool Remove(string key)
        {
            CheckReadOnly();
            ref TrieNode<TValue> node = ref GetNode(key, false);
            bool result = node.HasValue;
            if (result)
            {
                Count--;
                node.Value = default;
                node.HasValue = false;
            }
            return result;
        }

        public bool TryGetValue(string key, out TValue value)
        {
            ref TrieNode<TValue> node = ref GetNode(key, false);
            value = node.Value;
            return node.HasValue;
        }

        public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void CheckReadOnly()
        {
            if (IsReadOnly)
            {
                throw new InvalidOperationException("Trie is read-only.");
            }
        }

        // TrieInfo

        public TrieInfo GetInfo()
        {
            var result = new TrieInfo
            {
                Count = Count,
                IndexSize = _nodeIndexes.Length * sizeof(int),
                NodesSize = _nodes.Length * (sizeof(bool) + System.Runtime.InteropServices.Marshal.SizeOf<TValue>()),
                LookupSize = _keyIndexByCharacter.Length * sizeof(int)
            };
            return result;
        }
    }
}