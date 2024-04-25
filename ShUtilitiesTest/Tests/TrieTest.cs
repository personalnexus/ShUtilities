using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Collections;
using System.Collections.Generic;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class TrieTest: TrieTestBase<Trie<int>>
    {
        protected override Trie<int> CreateTrie() => new Trie<int>(PossibleCharacters, 10, 10);
    }
}
