﻿using BenchmarkDotNet.Attributes;
using ShUtilities.Collections;
using ShUtilities.Text;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShUtilitiesTest.Benchmarks
{
    public class DictionaryScannerBenchmark
    {

        [Benchmark(Baseline = true)]
        public void Split()
        {
            // Based on a real-world example with those idiosyncrasies
            var result = new Dictionary<string, string>();
            foreach (string keyValuePair in Input.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToList())
            {
                string[] keyAndValue = keyValuePair.Split('=');
                string key = keyAndValue[0];
                string value = keyAndValue[1];

                if (result.ContainsKey(key))
                {
                    result[key] = value;
                }
                else
                { 
                    result.Add(key, value); 
                }
            }
            if (result.Count != 100)
            {
                throw new InvalidOperationException($"Split: result.Count={result.Count}");
            }
        }

        [Benchmark]
        public void Scanner()
        {
            var result = new Dictionary<string, string>();
            DictionaryScanner.Parse(Input, result);
            if (result.Count != 100)
            {
                throw new InvalidOperationException($"Scanner: result.Count={result.Count}");
            }
        }

        [Benchmark]
        public void ScannerWithRelevantKeys()
        {
            var result = new Dictionary<string, string>();
            DictionaryScanner.Parse(Input, RelevantKeys, result);
            if (result.Count != 50)
            {
                throw new InvalidOperationException($"ScannerWithRelevantKeys: result.Count={result.Count}");
            }
        }

        private static readonly string Input = Enumerable.Range(0, 100).Select(i => $"Key{i}=Value{i}").ToDelimitedString("\r\n\n");

        private static readonly DictionaryRelevantKeys RelevantKeys = new DictionaryRelevantKeys(Enumerable.Range(0, 50).Select(i => $"Key{i*2}").ToArray());
    }
}
