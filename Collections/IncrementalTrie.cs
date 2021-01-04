using System.Collections.Generic;

namespace ShUtilities.Collections
{
    internal class IncrementalTrie<TValue> : Trie<TValue>
    {
        public IncrementalTrie(ISet<char> possibleCharacters, int initialCapacity, int capacityIncrement) : base(possibleCharacters, initialCapacity, capacityIncrement)
        {
        }

        internal void ClearValues()
        {
            for (int i = 0; i < _nodes.Length; i++)
            {
                _nodes[i].Value = default;
            }
        }

        internal bool TrySetValueByNodeIndexWithoutOverride(int nodeIndex, TValue value)
        {
            bool result;
            if (nodeIndex == 0)
            {
                result = false;
            }
            else
            {
                ref TrieNode node = ref _nodes[nodeIndex];
                result = node.HasValue && !node.Value.Equals(value);
                if (result)
                {
                    node.Value = value;
                }
            }
            return result;
        }

        internal bool ContainsIncremental(char character, ref int keyNodeIndex)
        {
            return TryGetNodeIndexIncremental(character, out int _, ref keyNodeIndex) == TrieNodeSearch.Found;
        }
    }
}
