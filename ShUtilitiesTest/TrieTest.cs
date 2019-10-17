using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Collections;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace ShUtilitiesTest
{
    [TestClass]
    public class TrieTest
    {
        [TestMethod]
        public void Remove_Clear_Count()
        {
            Trie<int> trie = CreateTrie();

            trie.Add("Value1", 1);
            Assert.AreEqual(1, trie.Count);

            trie["Value1"] = 2;
            Assert.AreEqual(1, trie.Count);

            Assert.IsTrue(trie.Remove("Value1"));
            Assert.AreEqual(0, trie.Count);

            trie.Add("Value1", 1);
            Assert.AreEqual(1, trie.Count);

            trie.Clear();
            Assert.AreEqual(0, trie.Count);
            Assert.IsFalse(trie.ContainsKey("Value1"));

            trie["Value1"] = 1;
            Assert.AreEqual(1, trie.Count);
            Assert.IsTrue(trie.TryGetValue("Value1", out int value));
            Assert.AreEqual(1, value);
        }

        [TestMethod]
        public void Add_TryGetValue()
        {
            Trie<int> trie = CreateTrie();
            trie.Add("Value1", 1);
            Assert.IsTrue(trie.TryGetValue("Value1", out int value1));
            Assert.AreEqual(1, value1);

            trie.Add("Val2", 2);
            Assert.IsTrue(trie.TryGetValue("Val2", out int value2));
            Assert.AreEqual(2, value2);
        }

        [TestMethod]
        public void ContainsKey()
        {
            Trie<int> trie = CreateTrie();
            trie.Add("Value1", 1);

            Assert.IsTrue(trie.ContainsKey("Value1"));
            Assert.IsFalse(trie.ContainsKey("Val"));
            Assert.IsFalse(trie.ContainsKey("Value2"));
        }

        [TestMethod]
        public void Performance()
        {
            const int ItemCount = 1_000_000;
            var trie = new Trie<int>(TrieCharacterSets.UpperCaseAndNumbers, ItemCount - 1000, 1000); // -1000 makes sure we trigger at least one resize operation
            PerformanceUtility.TimeAgainstDictionary(trie, ItemCount, 100, i => new KeyValuePair<string, int>(i.ToString("X"), i));
        }

        private Trie<int> CreateTrie()
        {
            var result = new Trie<int>(new HashSet<char> { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', 'V', 'a', 'l', 'u', 'e' }, 10, 10);
            return result;
        }
    }
}
