using BenchmarkDotNet.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace ShUtilitiesTest.Benchmarks
{
    public class DictionaryBenchmarkBase<TKey, TValue>
    {
        public DictionaryBenchmarkBase(IEnumerable<KeyValuePair<TKey, TValue>> expectedKeyValuePairs)
        {
            ExpectedKeyValuePairs = expectedKeyValuePairs.ToArray();
            _dictionary = AddExpectedKeyValuePairs(new Dictionary<TKey, TValue>());
        }

        protected KeyValuePair<TKey, TValue>[] ExpectedKeyValuePairs { get; }

        private readonly Dictionary<TKey, TValue> _dictionary;

        [Benchmark(Baseline = true)]
        public void Dictionary() => RunCore(_dictionary);

        protected void RunCore<TDictionary>(TDictionary dictionary)
            where TDictionary: IDictionary<TKey, TValue>
        {
            for (int i = 0; i < ExpectedKeyValuePairs.Length; i++)
            {
                KeyValuePair<TKey, TValue> keyValuePair = ExpectedKeyValuePairs[i];
                if (!dictionary.TryGetValue(keyValuePair.Key, out _))
                {
                    Assert.Fail($"{i} not found");
                }
                // TODO: uncomment to perform additional value check
                //else if (!keyValuePair.Value.Equals(value))
                //{
                //    Assert.Fail($"Value for key {i} should be {keyValuePair.Value} not {value}.");
                //}
            }
        }

        protected TDictionary AddExpectedKeyValuePairs<TDictionary>(TDictionary dictionary)
            where TDictionary: IDictionary<TKey, TValue>
        {
            foreach (KeyValuePair<TKey, TValue> item in ExpectedKeyValuePairs)
            {
                dictionary.Add(item.Key, item.Value);
            }
            return dictionary;
        }
    }
}
 