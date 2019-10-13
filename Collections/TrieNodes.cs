using System;

namespace ShUtilities.Collections
{
    internal class TrieNodes<TKey, TValue>
    {
        /// <summary>
        /// Large array with _possibleKeyCount slots for each node
        /// </summary>
        private TrieNode<TValue>[] _nodeSlots;

        private TrieNode<TValue> _rootNode;

        private byte _possibleKeyElementsCount;
        private int _currentOffset;
        private int _capacityIncrement = 1000; //TODO: fine-tune

        public TrieNodes(byte possibleKeyElementsCount, int initialCapacity)
        {
            _possibleKeyElementsCount = possibleKeyElementsCount;
            _nodeSlots = new TrieNode<TValue>[_possibleKeyElementsCount * initialCapacity];
            _rootNode = CreateNode();
        }


        private TrieNode<TValue> CreateNode(TrieNode<TValue> parent, byte index)
        {
            TrieNode<TValue> result = CreateNode();
            int nodeSlot = parent.Offset + index;
            if (nodeSlot >= _nodeSlots.Length)
            {
                Array.Resize(ref _nodeSlots, nodeSlot + (_possibleKeyElementsCount * _capacityIncrement));
            }
            _nodeSlots[nodeSlot] = result;
            return result;
        }

        private TrieNode<TValue> CreateNode()
        {
            var result = new TrieNode<TValue>(_currentOffset);
            _currentOffset += _possibleKeyElementsCount;
            return result;
        }

        private TrieNode<TValue> GetNode(TrieNode<TValue> parent, byte index)
        {
            // When a node is created, _nodeSlots is ensured to have enough slots to store all
            // its children, so no range check is needed when accessing the array
            TrieNode<TValue> result = _nodeSlots[parent.Offset + index];
            return result;
        }

        internal bool TryGet(TrieKey key, out TrieNode<TValue> node)
        {
            TrieNode<TValue> parentNode = _rootNode;
            node = null;
            foreach (byte index in key.Indexes)
            {
                node = GetNode(parentNode, index);
                parentNode = node;
            }
            return node != null;
        }

        internal TrieNode<TValue> GetOrAdd(TrieKey key)
        {
            TrieNode<TValue> parentNode = _rootNode;
            TrieNode<TValue> currentNode = _rootNode;
            foreach (byte index in key.Indexes)
            {
                currentNode = GetNode(parentNode, index) ?? CreateNode(parentNode, index);
                parentNode = currentNode;
            }
            return currentNode;
        }
    }
}
