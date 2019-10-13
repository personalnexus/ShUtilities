using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Collections;
using System.Linq;
using System.Collections.Generic;

namespace ShUtilitiesTest
{
    [TestClass]
    public class TrieTest
    {
        [TestMethod]
        public void Add_TryGetValue()
        {
            Trie<string, char, int> trie = CreateTrie();
            TrieKey key = trie.Add("Value1", 1);
            Assert.IsNotNull(key);
            Assert.IsTrue(trie.TryGetValue(key, out int value1));
            Assert.AreEqual(1, value1);
        }

        [TestMethod]
        public void ContainsKey()
        {
            Trie<string, char, int> trie = CreateTrie();
            TrieKey key1 = trie.Add("Value1", 1);
            TrieKey key2 = trie.CreateKey("Value2");

            Assert.IsTrue(trie.ContainsKey(key1));
            Assert.IsFalse(trie.ContainsKey(key2));

            Assert.IsTrue(trie.ContainsKey("Value1"));
            Assert.IsFalse(trie.ContainsKey("Value2"));
        }

        [TestMethod]
        public void CreateKey_GetKeyElements()
        {
            Trie<string, char, int> trie = CreateTrie();
            TrieKey key = trie.CreateKey("Value2");
            IEnumerable<char> keyElements = trie.GetKeyElements(key);
            Assert.AreEqual("Value2", new string(keyElements.ToArray()));
        }

        [TestMethod]
        public void CompareTrieKeys()
        {
            Trie<string, char, int> trie = CreateTrie();
            TrieKey key1a = trie.CreateKey("Value1");
            TrieKey key1b = trie.CreateKey("Value1");

            Assert.AreNotSame(key1a, key1b);
            Assert.AreEqual(key1a, key1b);
        }

        [TestMethod]
        public void CompareCachedTrieKeys()
        {
            Trie<string, char, int> trie = CreateTrie();
            var cache = new TrieKeyCache<string, char, int>(trie);
            TrieKey key1a = cache.CreateKey("Value1");
            TrieKey key1b = cache.CreateKey("Value1");

            Assert.AreSame(key1a, key1b);
            Assert.AreEqual(key1a, key1b);
        }

        private Trie<string, char, int> CreateTrie()
        {
            var result = new Trie<string, char, int>(new HashSet<char> { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', 'V', 'a', 'l', 'u', 'e' }, 10);
            return result;
        }
    }
}
