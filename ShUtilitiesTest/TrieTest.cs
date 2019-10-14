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
        public void Add_TryGetValue()
        {
            Trie<string, char, int> trie = CreateTrie();
            TrieKey key1 = trie.Add("Value1", 1);
            Assert.IsNotNull(key1);
            Assert.IsTrue(trie.TryGetValue(key1, out int value1));
            Assert.AreEqual(1, value1);

            TrieKey key2 = trie.Add("Val2", 2);
            Assert.IsNotNull(key2);
            Assert.IsTrue(trie.TryGetValue(key2, out int value2));
            Assert.AreEqual(2, value2);

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

            Assert.AreEqual(key1a, key1b);
        }

        [TestMethod]
        public void CompareCachedTrieKeys()
        {
            Trie<string, char, int> trie = CreateTrie();
            var cache = new TrieKeyCache<string, char, int>(trie);
            TrieKey key1a = cache.CreateKey("Value1");
            TrieKey key1b = cache.CreateKey("Value1");

            Assert.AreEqual(key1a, key1b);
        }

        [TestMethod]
        public void Performance()
        {
            // Initialization
            var stopwatchInitialization = new Stopwatch();
            stopwatchInitialization.Start();

            const int ItemCount = 1_000_000;
            
            ISet<char> numbersAndLetters = Enumerable.Range('A', 6).Union(Enumerable.Range('0', 10)).Select(i => (char)i).ToHashSet();
            var trie = new Trie<string, char, int>(numbersAndLetters, ItemCount);
            var trieKeys = new TrieKey[ItemCount];

            var dictionary = new Dictionary<string, int>(ItemCount);
            var dictionaryKeys = new string[ItemCount];

            for (int i = 0; i < ItemCount; i++)
            {
                string key = i.ToString("X");
                dictionaryKeys[i] = key;
                dictionary.Add(key, i);
                trieKeys[i] = trie.Add(key, i);
            }
            stopwatchInitialization.Stop();
            Console.WriteLine($"{stopwatchInitialization.ElapsedMilliseconds} Initialization");

            // Dictionary
            var stopwatchDictionary = new Stopwatch();
            stopwatchDictionary.Start();
            for (int counter = 0; counter < 100; counter++)
            {
                for (int i = 0; i < ItemCount; i++)
                {
                    if (!dictionary.TryGetValue(dictionaryKeys[i], out int value))
                    {
                        Assert.Fail($"{i} not found");
                    }
                    else if (value > ItemCount)
                    {
                        Assert.Fail($"{value} for key {i} must not be greater than {ItemCount}");
                    }
                }
            }
            stopwatchDictionary.Stop();
            Console.WriteLine($"{stopwatchDictionary.ElapsedMilliseconds} Dictionary");

            // Trie
            var stopwatchTrie = new Stopwatch();
            stopwatchTrie.Start();
            for (int counter = 0; counter < 100; counter++)
            {
                for (int i = 0; i < ItemCount; i++)
                {
                    if (!trie.TryGetValue(trieKeys[i], out int value))
                    {
                        Assert.Fail($"{i} not found");
                    }
                    else if (value > ItemCount)
                    {
                        Assert.Fail($"{value} for key {i} must not be greater than {ItemCount}");
                    }
                }
            }
            stopwatchTrie.Stop();
            Console.WriteLine($"{stopwatchTrie.ElapsedMilliseconds} Trie");
            Console.WriteLine($"{trie.GetInfo().NodeCount} TrieNodes");

        }

        private Trie<string, char, int> CreateTrie()
        {
            var result = new Trie<string, char, int>(new HashSet<char> { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', 'V', 'a', 'l', 'u', 'e' }, 10);
            return result;
        }
    }
}
