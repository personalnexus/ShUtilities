namespace ShUtilities.Collections
{
    internal struct TrieNode<TValue>
    {
        public TrieNode(int childrenStartIndex)
        {
            ChildrenStartIndex = childrenStartIndex;
            Value = default;
        }

        /// <summary>
        /// Offset in the _childNodeIndexes indicating where child node indexes are stored
        /// </summary>
        public readonly int ChildrenStartIndex;
        public TValue Value;

        internal bool IsAssigned() => ChildrenStartIndex != 0;
    }
}
