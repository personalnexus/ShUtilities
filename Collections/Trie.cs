using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ShUtilities.Collections
{
    public class Trie<TValue>: IDictionary<string, TValue>
    {
        public Trie(ISet<char> possibleCharacters, int initialCapacity, int capacityIncrement)
        {
            // populate the lookup with an impossible key index, so we can tell later when the key contains an invalid character
            int maxCharacter = possibleCharacters.Max();
            _keyIndexByCharacter = new int[maxCharacter + 1];
            Array.Fill(_keyIndexByCharacter, InvalidKeyIndex);

            int index = InvalidKeyIndex + 1;
            foreach (char character in possibleCharacters)
            {
                _keyIndexByCharacter[character] = index;
                index++;
            }

            _capacityIncrement = capacityIncrement;
            _possibleCharacterCount = possibleCharacters.Count;
            Resize(initialCapacity);
        }

        // Key lookup

        private readonly int[] _keyIndexByCharacter;
        private const int InvalidKeyIndex = -1;

        // Nodes

        private protected TrieNode[] _nodes;
        private int[] _nodeIndexes;
        private int _lastUsedNodeIndex;

        private readonly int _possibleCharacterCount;
        private readonly int _capacityIncrement;

        private ref TrieNode GetNode(string key, bool createIfMissing)
        {
            int nodeIndex = 0;
            foreach (char character in key)
            {
                TrieNodeSearch nodeSearch = TryGetNodeIndexIncremental(character, out int indexIndex, ref nodeIndex);
                if (nodeSearch == TrieNodeSearch.Invalid)
                {
                    throw new ArgumentOutOfRangeException("Key contains characters unsupported by this Trie");
                }
                else if (nodeSearch == TrieNodeSearch.NotFound)
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

        private protected TrieNodeSearch TryGetNodeIndexIncremental(char character, out int indexIndex, ref int nodeIndex)
        {
            TrieNodeSearch result;
            int keyIndex;
            if ((int)character > _keyIndexByCharacter.Length || (keyIndex = _keyIndexByCharacter[character]) == InvalidKeyIndex)
            {
                result = TrieNodeSearch.Invalid;
                indexIndex = 0;
            }
            else
            {
                // Step 1: get the index of where in the _indexes array the index into _nodes is found
                indexIndex = (nodeIndex * _possibleCharacterCount) + keyIndex;
                // Step 2: get the index of the value in _values
                nodeIndex = _nodeIndexes[indexIndex];
                result = nodeIndex == 0 ? TrieNodeSearch.NotFound : TrieNodeSearch.Found;
            }
            return result;
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
            ref TrieNode node = ref GetNode(key, true);
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
            _nodes = new TrieNode[1];
            _nodeIndexes = new int[_possibleCharacterCount];
            _lastUsedNodeIndex = 0;
            Count = 0;
        }

        public bool ContainsKey(string key)
        {
            ref TrieNode node = ref GetNode(key, false);
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
            ref TrieNode node = ref GetNode(key, false);
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
            ref TrieNode node = ref GetNode(key, false);
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

        private protected void CheckReadOnly()
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

        private protected struct TrieNode
        {
            public TValue Value;
            /// <remarks>
            /// We could store these flags in a BitArray instead. But in preliminary tests this accessing a second array for each operation on values 
            /// has resulted in worse performance, possibly because this has was better data locality and requires fewer array look-ups.
            /// </remarks>
            public bool HasValue;
        }

    }
}