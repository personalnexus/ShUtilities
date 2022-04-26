using BenchmarkDotNet.Running;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilitiesTest.Benchmarks;
using ShUtilitiesTest.Benchmarks.Configuration;
using System.Diagnostics;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class BenchmarkTests
    {
        [TestMethod]
        public void Trie_UnsafeTrie_Dictionary() => Run<TrieBenchmark>();

        [TestMethod]
        public void ByteDictionary_Dictionary() => Run<ByteDictionaryBenchmark>();

        [TestMethod]
        public void ShortDictionary_Dictionary() => Run<ShortDictionaryBenchmark>();

        [TestMethod]
        public void DictionaryScanner_Split() => Run<DictionaryScannerBenchmark>();

        [TestMethod]
        public void PropertyChanged() => Run<PropertyChangedBenchmark>();

        private static void Run<T>()
        {
            Debug.Assert(false, "Benchmarks must not be run in debug configuration");
            BenchmarkRunner.Run<T>(new MinimalConfiguration());
        }
    }
}
