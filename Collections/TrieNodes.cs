using System;

namespace ShUtilities.Collections
{
    internal class TrieNodes<TKey, TValue>
    {
        private TrieNode<TValue>[] _nodes;
        private int _currentNodeIndex;

        /// <summary>
        /// Instead of storing a small array on each TrieNode with the child indexes, each TrieNode uses a section of this
        /// large array
        /// </summary>
        private int[] _childNodeIndexes;

        private readonly int _possibleChildrenCount;
        private readonly int _capacityIncrement;

        public TrieNodes(int possibleChildrenCount) : this(possibleChildrenCount, 1000, 1000)
        {
        }

        public TrieNodes(int possibleChildrenCount, int initialCapacity) : this(possibleChildrenCount, initialCapacity, 1000)
        {
        }

        public TrieNodes(int possibleChildrenCount, int initialCapacity, int capacityIncrement)
        {
            _capacityIncrement = capacityIncrement;
            _possibleChildrenCount = possibleChildrenCount;
            _currentNodeIndex = -1;
            Resize(initialCapacity);
            Create();
        }

        public ref TrieNode<TValue> Get(ReadOnlySpan<int> indexes, bool createIfNotAssigned)
        {
            ref TrieNode<TValue> node = ref _nodes[0];
            int nodeIndex = 0;
            for (int i = 0; i < indexes.Length; i++)
            {
                int childIndex = node.ChildrenStartIndex + indexes[i]; // Step 1: get where in the _childNodeIndexes array the child node's index in _nodes is found
                nodeIndex = _childNodeIndexes[childIndex];         // Step 2: get the index of the child node in _nodes
                if (nodeIndex != 0)
                {
                    node = ref _nodes[nodeIndex];
                }
                else
                {
                    if (createIfNotAssigned)
                    {
                        node = ref Create();
                        _childNodeIndexes[childIndex] = _currentNodeIndex; // Register the child in the parent's section of _childNodeIndexes
                    }
                    else
                    {
                        node = ref _nodes[0];
                        break;
                    }
                }
            }
            return ref node;
        }

        private ref TrieNode<TValue> Create()
        {
            _currentNodeIndex++;
            _nodes[_currentNodeIndex] = new TrieNode<TValue>(_currentNodeIndex * _possibleChildrenCount);
            ref TrieNode<TValue> result = ref _nodes[_currentNodeIndex];
            if (_currentNodeIndex == _nodes.Length - 1)
            {
                Resize(_capacityIncrement);
            }
            return ref result;
        }

        private void Resize(int additionalNodeCount)
        {
            int newLength = (_nodes?.Length ?? 0) + additionalNodeCount;
            Array.Resize(ref _nodes, newLength);
            Array.Resize(ref _childNodeIndexes, newLength * _possibleChildrenCount);
        }

        public int Count => _nodes.Length;
        public int IndexSize => _childNodeIndexes.Length * sizeof(int);
    }
}
