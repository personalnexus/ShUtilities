namespace ShUtilities.Collections
{
    internal class TrieNode<TValue>
    {
        public TrieNode(int offset)
        {
            Offset = offset;
        }

        public int Offset { get; }
        public TValue Value { get; set; }
    }
}
