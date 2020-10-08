using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Collections;
using System.Collections.Generic;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class UnsafeTrieTest: TrieTestBase<UnsafeTrie<int>>
    {
        [TestMethod]
        public void Performance()
        {
            var unsafeTrie = new UnsafeTrie<int>(TrieCharacterSets.UpperCaseAndNumbers, ItemCount - 1000, 1000);
            var trie = new Trie<int>(TrieCharacterSets.UpperCaseAndNumbers, ItemCount - 1000, 1000);
            PerformanceUtility.TimeDictionaries(unsafeTrie, trie, ItemCount, 100, i => new KeyValuePair<string, int>(i.ToString("X"), i));
        }

        protected override UnsafeTrie<int> CreateTrie()
        {
            var result = new UnsafeTrie<int>(new HashSet<char> { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', 'V', 'a', 'l', 'u', 'e' }, 10, 10);
            return result;
        }
    }
}
