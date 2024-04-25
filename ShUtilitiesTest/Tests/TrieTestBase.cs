using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ShUtilitiesTest.Tests
{
    public abstract class TrieTestBase<T>
            where T: IDictionary<string, int>
    {
        protected static readonly ISet<char> PossibleCharacters = new HashSet<char>("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789");

        [TestMethod]
        public void Remove_Clear_Count()
        {
            T trie = CreateTrie();

            trie.Add("VALUE1", 1);
            Assert.AreEqual(1, trie.Count);

            trie["VALUE1"] = 2;
            Assert.AreEqual(1, trie.Count);

            Assert.IsTrue(trie.Remove("VALUE1"));
            Assert.AreEqual(0, trie.Count);

            trie.Add("VALUE1", 1);
            Assert.AreEqual(1, trie.Count);

            trie.Clear();
            Assert.AreEqual(0, trie.Count);
            Assert.IsFalse(trie.ContainsKey("VALUE  1"));

            trie["VALUE1"] = 1;
            Assert.AreEqual(1, trie.Count);
            Assert.IsTrue(trie.TryGetValue("VALUE1", out int value));
            Assert.AreEqual(1, value);
        }

        [TestMethod]
        public void Add_TryGetValue()
        {
            T trie = CreateTrie();
            trie.Add("VALUE1", 1);
            Assert.IsTrue(trie.TryGetValue("VALUE1", out int value1));
            Assert.AreEqual(1, value1);

            trie.Add("VAL2", 2);
            Assert.IsTrue(trie.TryGetValue("VAL2", out int value2));
            Assert.AreEqual(2, value2);
        }

        [TestMethod]
        public void AddAllPossibleCharacters()
        {
            // Arrange
            T trie = CreateTrie();
            
            // Act
            foreach (char character in PossibleCharacters)
            {
                trie.Add(character.ToString() + character.ToString(), (int)character);
            }

            // Assert
            trie.Count.Should().Be(PossibleCharacters.Count);
        }

        [TestMethod]
        public void ContainsKey()
        {
            T trie = CreateTrie();
            trie.Add("VALUE1", 1);

            Assert.IsTrue(trie.ContainsKey("VALUE1"));
            Assert.IsFalse(trie.ContainsKey("VAL"));
            Assert.IsFalse(trie.ContainsKey("VALUE2"));
        }

        protected abstract T CreateTrie();
    }
}
