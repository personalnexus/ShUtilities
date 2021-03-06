﻿using BenchmarkDotNet.Running;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilitiesTest.Benchmarks;
using ShUtilitiesTest.Benchmarks.Configuration;
using System.Diagnostics;

namespace ShUtilitiesTest
{
    [TestClass]
    public class BenchmarkTests
    {
        [TestMethod]
        public void Trie_UnsafeTrie_Dictionary() => Run<TrieBenchmark>();

        [TestMethod]
        public void ByteDictionary_Dictionary() => Run<ByteDictionaryBenchmark>();

        [TestMethod]
        public void DictionaryScanner_Split() => Run<DictionaryScannerBenchmark>();

        private static void Run<T>()
        {
            Debug.Assert(false, "Benchmarks must not be run in debug configuration");
            BenchmarkRunner.Run<T>(new MinimalConfiguration());
        }
    }
}
