namespace ShUtilities.Collections
{
    public class TrieInfo
    {
        public int NodeCount { get; internal set; }
        public int IndexSize { get; internal set; }

        public override string ToString()
        {
            return $"{nameof(NodeCount)}: {NodeCount}, {nameof(IndexSize)}: {IndexSize}";
        }
    }
}
