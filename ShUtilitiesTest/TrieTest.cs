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
            // Initialization
            var stopwatchInitialization = new Stopwatch();
            stopwatchInitialization.Start();

            const int ItemCount = 1_000_000;
            
            ISet<char> numbersAndLetters = Enumerable.Range('A', 26).Union(Enumerable.Range('0', 10)).Select(i => (char)i).ToHashSet();
            var trie = new Trie<int>(numbersAndLetters, ItemCount - 1000, 1000); // -1000 makes sure we trigger at least one resize operation
            var dictionary = new Dictionary<string, int>(ItemCount);
            var keys = new string[ItemCount];

            for (int i = 0; i < ItemCount; i++)
            {
                string key = i.ToString("X");
                keys[i] = key;
                dictionary.Add(key, i);
                trie.Add(key, i);
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
                    if (!dictionary.TryGetValue(keys[i], out int value))
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
                    if (!trie.TryGetValue(keys[i], out int value))
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
            Console.WriteLine($"{trie.GetInfo()}");

        }

        private Trie<int> CreateTrie()
        {
            var result = new Trie<int>(new HashSet<char> { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', 'V', 'a', 'l', 'u', 'e' }, 10, 10);
            return result;
        }
    }
}
