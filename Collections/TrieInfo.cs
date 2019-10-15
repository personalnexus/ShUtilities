namespace ShUtilities.Collections
{
    public class TrieInfo
    {
        public int Count { get; internal set; }
        public int IndexSize { get; internal set; }
        public int NodesSize { get; internal set; }
        public int LookupSize { get; internal set; }

        public override string ToString()
        {
            return $"{nameof(Count)}: {Count}, {nameof(IndexSize)}: {IndexSize}, {nameof(NodesSize)}: {NodesSize}, {nameof(LookupSize)}: {LookupSize}";
        }
    }
}
