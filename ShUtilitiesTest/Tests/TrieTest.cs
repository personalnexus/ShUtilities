using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Collections;
using System.Collections.Generic;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class TrieTest: TrieTestBase<Trie<int>>
    {
        protected override Trie<int> CreateTrie()
        {
            var result = new Trie<int>(new HashSet<char> { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', 'V', 'a', 'l', 'u', 'e' }, 10, 10);
            return result;
        }
    }
}
