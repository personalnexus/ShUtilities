namespace ShUtilities.Collections
{
    internal struct TrieNode<TValue>
    {
        public TValue Value;
        /// <remarks>
        /// We could store these flags in a BitArray instead. But in preliminary tests this accessing a second array for each operation on values 
        /// has resulted in worse performance, possibly because this has was better data locality and requires fewer array look-ups.
        /// </remarks>
        public bool HasValue;
    }
}
