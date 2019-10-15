using System;
using System.Collections.Generic;

namespace ShUtilities.Collections
{
    public class Trie<TValue>
    {
        public Trie(ISet<char> possibleCharacters, int initialCapacity, int capacityIncrement)
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

        private int[] _keyIndexByCharacter;

        // Nodes

        private TrieNode<TValue>[] _nodes;
        private int[] _nodeIndexes;
        private int _lastUsedNodeIndex;

        private readonly int _possibleCharacterCount;
        private readonly int _capacityIncrement;

        private ref TrieNode<TValue> GetNode(string key, bool createIfMissing)
        {
            int nodeIndex = 0;
            foreach (char character in key)
            {
                // Step 1: get the index of where in the _indexes array the index into _nodes is found
                int indexIndex = (nodeIndex * _possibleCharacterCount) + _keyIndexByCharacter[character];
                // Step 2: get the index of the value in _values
                nodeIndex = _nodeIndexes[indexIndex];
                if (nodeIndex == 0)
                {
                    if (createIfMissing)
                    {
                        _lastUsedNodeIndex++;
                        if (_lastUsedNodeIndex == _nodes.Length)
                        {
                            Resize(_nodes.Length + _capacityIncrement);
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

        public void Add(string key, TValue value)
        {
            ref TrieNode<TValue> node = ref GetNode(key, true);
            node.Value = value;
            node.HasValue = true;
        }

        public bool TryGetValue(string key, out TValue value)
        {
            ref TrieNode<TValue> node = ref GetNode(key, false);
            value = node.Value;
            return node.HasValue;
        }

        public bool ContainsKey(string key)
        {
            ref TrieNode<TValue> node = ref GetNode(key, false);
            return node.HasValue;
        }

        // TrieInfo

        public TrieInfo GetInfo()
        {
            var result = new TrieInfo
            {
                Count = _lastUsedNodeIndex,
                IndexSize = _nodeIndexes.Length * sizeof(int),
                NodesSize = _nodes.Length * (sizeof(bool) + System.Runtime.InteropServices.Marshal.SizeOf<TValue>()),
                LookupSize = _keyIndexByCharacter.Length * sizeof(int)
            };
            return result;
        }
    }
}