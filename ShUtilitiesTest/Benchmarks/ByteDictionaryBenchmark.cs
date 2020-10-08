using BenchmarkDotNet.Attributes;
using System.Collections.Generic;
using ShUtilities.Collections;
using System.Linq;

namespace ShUtilitiesTest.Benchmarks
{
    public class ByteDictionaryBenchmark: DictionaryBenchmarkBase<byte, int>
    {
        public ByteDictionaryBenchmark(): base(Enumerable.Range(0, 255).Select(i => new KeyValuePair<byte, int>((byte)i, i)))
        {
            _byteDictionary = AddExpectedKeyValuePairs(new ByteDictionary<int>());
        }

        private readonly ByteDictionary<int> _byteDictionary;

        [Benchmark]
        public void ByteDictionary() => RunCore(_byteDictionary);
    }
}
