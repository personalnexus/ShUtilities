using System;

namespace ShUtilities.Collections
{
    internal class TrieNodes<TKey, TValue>
    {
        /// <summary>
        /// Large array with (1+_possibleChildrenCount) slots for each node
        /// </summary>
        private TrieNode<TValue>[] _nodes;

        private readonly byte _possibleChildrenCount;
        private readonly int _capacityIncrement;
        private int _currentChildrenStartIndex;
        
        public TrieNodes(byte possibleChildrenCount, int initialCapacity): this(possibleChildrenCount, initialCapacity, 1000)
        {
        }

        public TrieNodes(byte possibleChildrenCount, int initialCapacity, int capacityIncrement)
        {
            _capacityIncrement = capacityIncrement;
            _possibleChildrenCount = possibleChildrenCount;
            Resize(initialCapacity);
            Create(0);
        }

        public ref TrieNode<TValue> Get(byte[] indexes, bool createIfNotAssigned)
        {
            ref TrieNode<TValue> node = ref _nodes[0];
            for (int i = 0; i < indexes.Length; i++)
            {
                int nodeIndex = node.ChildrenStartIndex + indexes[i];
                node = ref _nodes[nodeIndex];
                if (!node.IsAssigned())
                {
                    if (createIfNotAssigned)
                    {
                        node = ref Create(nodeIndex);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return ref node;
        }

        private ref TrieNode<TValue> Create(int nodeIndex)
        {
            _nodes[nodeIndex] = new TrieNode<TValue>(_currentChildrenStartIndex);
            _currentChildrenStartIndex += (1 + _possibleChildrenCount);
            if (_currentChildrenStartIndex >= _nodes.Length)
            {
                Resize(_capacityIncrement);
            }
            return ref _nodes[nodeIndex];
        }

        private void Resize(int additionalNodeCount)
        {
            int length = _nodes?.Length ?? 0;
            length += ((1 + _possibleChildrenCount) * additionalNodeCount);
            Array.Resize(ref _nodes, length);
        }

        public int Count => _nodes.Length;

    }
}
