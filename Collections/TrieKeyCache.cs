using System.Collections.Generic;

namespace ShUtilities.Collections
{
    public class TrieKeyCache<TKey, TKeyElement, TValue>
        where TKey: IEnumerable<TKeyElement>
    {
        private readonly Trie<TKey, TKeyElement, TValue> _trie;
        private readonly Dictionary<TKey, TrieKey> _trieKeysByKey;

        public TrieKeyCache(Trie<TKey, TKeyElement, TValue> trie)
        {
            _trie = trie;
            _trieKeysByKey = new Dictionary<TKey, TrieKey>();
        }

        public TrieKey CreateKey(TKey key)
        {
            TrieKey result = _trieKeysByKey.GetOrAdd(key, _trie.CreateKey);
            return result;
        }
    }
}
