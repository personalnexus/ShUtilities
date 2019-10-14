namespace ShUtilities.Collections
{
    internal struct TrieNode<TValue>
    {
        public TrieNode(int childrenStartIndex)
        {
            ChildrenStartIndex = childrenStartIndex;
            Value = default;
        }

        public readonly int ChildrenStartIndex;
        public TValue Value;

        internal bool IsAssigned() => ChildrenStartIndex != 0;
    }
}
