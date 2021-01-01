using BenchmarkDotNet.Attributes;
using ShUtilities.Collections;
using ShUtilities.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShUtilitiesTest.Benchmarks
{
    public class DictionaryScannerBenchmark
    {

        [Benchmark]
        public void Split()
        {
            Dictionary<string, string> result = ParseWithSplit();
            if (result.Count != 100)
            {
                throw new InvalidOperationException($"Split: result.Count={result.Count}");
            }
        }

        [Benchmark]
        public void Scanner()
        {
            var result = DictionaryScanner.Parse(Input);
            if (result.Count != 100)
            {
                throw new InvalidOperationException($"Scanner: result.Count={result.Count}");
            }
        }

        private static readonly string Input = Enumerable.Range(0, 100).Select(i => $"Key{i}=Value{i}").ToDelimitedString("\n");

        private static Dictionary<string, string> ParseWithSplit()
        {
            var result = new Dictionary<string, string>();
            foreach (string keyValuePair in Input.Split('\n'))
            {
                string[] keyAndValue = keyValuePair.Split('=');
                result.Add(keyAndValue[0], keyAndValue[1]);
            }

            return result;
        }
    }
}
