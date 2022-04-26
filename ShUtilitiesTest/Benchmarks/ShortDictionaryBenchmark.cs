using BenchmarkDotNet.Attributes;
using System.Collections.Generic;
using ShUtilities.Collections;
using System.Linq;

namespace ShUtilitiesTest.Benchmarks
{
    public class ShortDictionaryBenchmark : DictionaryBenchmarkBase<short, int>
    {
        public ShortDictionaryBenchmark() : base(Enumerable.Range(-5, 10).Select(i => new KeyValuePair<short, int>((short)i, i)))
        {
            _shortDictionary = AddExpectedKeyValuePairs(new ShortDictionary<int>());
        }

        private readonly ShortDictionary<int> _shortDictionary;

        [Benchmark]
        public void ShortDictionary() => RunCore(_shortDictionary);
    }
}
