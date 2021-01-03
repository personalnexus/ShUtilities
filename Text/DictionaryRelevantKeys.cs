using ShUtilities.Collections;

namespace ShUtilities.Text
{
    /// <summary>
    /// A thin wrapper around a <see cref="Trie{bool}"/> that is used for incrementally searching to determine whether a key-value-pair is relevant
    /// </summary>
    public struct DictionaryRelevantKeys
    {
        private Trie<bool> _isValueSetByKey;

        public DictionaryRelevantKeys(params string[] relevantKeys)
        {
            _isValueSetByKey = new Trie<bool>(TrieCharacterSets.FromStrings(relevantKeys), relevantKeys.Length, 1);
            foreach (string key in relevantKeys)
            {
                _isValueSetByKey.Add(key, false);
            }
        }

        internal bool Contains(char character, ref int keyNodeIndex)
        {
            bool result = _isValueSetByKey.TryGetNodeIndexIncremental(character, out int _, ref keyNodeIndex) == TrieNodeSearch.Found;
            return result;
        }

        internal void Clear() => _isValueSetByKey.ClearValues();

        internal int Count => _isValueSetByKey.Count;

        internal bool SetValue(int keyNodeIndex, ref int valueCount)
        {
            bool result;
            if (keyNodeIndex != 0 && 
                _isValueSetByKey.TrySetValueByNodeIndexWithoutOverride(keyNodeIndex, true))
            {
                valueCount++;
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
    }
}
