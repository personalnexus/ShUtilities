using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ShUtilitiesTest.Tests
{
    public abstract class TrieTestBase<T>
            where T: IDictionary<string, int>
    {
        protected const int ItemCount = 1_000_000;

        [TestMethod]
        public void Remove_Clear_Count()
        {
            T trie = CreateTrie();

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
            T trie = CreateTrie();
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
            T trie = CreateTrie();
            trie.Add("Value1", 1);

            Assert.IsTrue(trie.ContainsKey("Value1"));
            Assert.IsFalse(trie.ContainsKey("Val"));
            Assert.IsFalse(trie.ContainsKey("Value2"));
        }

        protected abstract T CreateTrie();
    }
}
