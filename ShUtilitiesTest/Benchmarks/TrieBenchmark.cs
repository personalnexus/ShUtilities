using BenchmarkDotNet.Attributes;
using System.Collections.Generic;
using ShUtilities.Collections;
using System.Linq;

namespace ShUtilitiesTest.Benchmarks
{
    public class TrieBenchmark : DictionaryBenchmarkBase<string, int>
    {
        public TrieBenchmark() : base(Enumerable.Range(0, ItemCount).Select(i => new KeyValuePair<string, int>(i.ToString("X"), i)))
        {
            // make sure we trigger at least one resize operation
            _trie = AddExpectedKeyValuePairs(new Trie<int>(PossibleCharacters, ItemCount - 2, 1000));
            _unsafeTrie = AddExpectedKeyValuePairs(new UnsafeTrie<int>(PossibleCharacters, ItemCount - 2, 1000));
        }

        private const int ItemCount = 0x11111;
        private readonly HashSet<char> PossibleCharacters = new HashSet<char>("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789");

        private readonly Trie<int> _trie;
        private readonly UnsafeTrie<int> _unsafeTrie;

        [Benchmark]
        public void UnsafeTrie() => RunCore(_unsafeTrie);

        [Benchmark]
        public void Trie() => RunCore(_trie);
    }
}
