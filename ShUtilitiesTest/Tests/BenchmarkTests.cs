using BenchmarkDotNet.Running;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilitiesTest.Benchmarks;
using ShUtilitiesTest.Benchmarks.Configuration;

namespace ShUtilitiesTest
{
    [TestClass]
    public class BenchmarkTests
    {
        [TestMethod]
        public void Trie_UnsafeTrie_Dictionary()
        {
            BenchmarkRunner.Run<TrieBenchmark>(new MinimalConfiguration());
        }

        [TestMethod]
        public void ByteDictionary_Dictionary()
        {
            BenchmarkRunner.Run<ByteDictionaryBenchmark>(new MinimalConfiguration());
        }
    }
}
