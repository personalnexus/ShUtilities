using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Collections;
using System.Collections.Generic;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class UnsafeTrieTest: TrieTestBase<UnsafeTrie<int>>
    {
        protected override UnsafeTrie<int> CreateTrie() => new UnsafeTrie<int>(PossibleCharacters, 10, 10);
    }
}
