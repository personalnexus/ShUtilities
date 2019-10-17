using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ShUtilitiesTest
{
    public static class PerformanceUtility
    {
        public static void TimeAgainstDictionary<TFast, TKey, TValue>(TFast fastDictionary, int itemCount, int iterationCount, Func<int, KeyValuePair<TKey, TValue>> keyValuePairGenerator)
            where TFast : IDictionary<TKey, TValue>
        {
            var slowDictionary = new Dictionary<TKey, TValue>(itemCount);
            TimeDictionaries(fastDictionary, slowDictionary, itemCount, iterationCount, keyValuePairGenerator);
        }
            
        public static void TimeDictionaries<TFast, TSlow, TKey, TValue>(TFast fastDictionary, TSlow slowDictionary, int itemCount, int iterationCount, Func<int, KeyValuePair<TKey, TValue>> keyValuePairGenerator)
            where TFast : IDictionary<TKey, TValue>
            where TSlow : IDictionary<TKey, TValue>
        {
            KeyValuePair<TKey, TValue>[] expectedKeysAndValues = Enumerable.Range(0, itemCount).Select(x => keyValuePairGenerator(x)).ToArray();

            (TimeSpan fastTime, TimeSpan slowTime) = TimeDictionaries(fastDictionary, slowDictionary, expectedKeysAndValues, iterationCount);

            Console.WriteLine($"{fastTime} {typeof(TFast).Name}");
            Console.WriteLine($"{slowTime} {typeof(TSlow).Name}");
            Console.WriteLine($"{(1 - (double)fastTime.Ticks / slowTime.Ticks):p} saved");

            Assert.IsTrue(fastTime < slowTime, $"{typeof(TFast).Name} was slower than {typeof(TSlow).Name}???");
        }

        public static (TimeSpan time1, TimeSpan time2) TimeDictionaries<T1, T2, TKey, TValue>(T1 dictionary1, T2 dictionary2, KeyValuePair<TKey, TValue>[] expectedKeysAndValues, int iterations)
            where T1 : IDictionary<TKey, TValue>
            where T2 : IDictionary<TKey, TValue>
        {
            TimeSpan time1 = TimeDictionary(dictionary1, expectedKeysAndValues, iterations);
            TimeSpan time2 = TimeDictionary(dictionary2, expectedKeysAndValues, iterations);
            return (time1, time2);
        }

        public static TimeSpan TimeDictionary<T, TKey, TValue>(T dictionary, KeyValuePair<TKey, TValue>[] expectedKeysAndValues, int iterationCount)
            where T : IDictionary<TKey, TValue>
        {
            for (int i = 0; i < expectedKeysAndValues.Length; i++)
            {
                dictionary.Add(expectedKeysAndValues[i].Key, expectedKeysAndValues[i].Value);
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int counter = 0; counter < iterationCount; counter++)
            {
                for (int i = 0; i < expectedKeysAndValues.Length; i++)
                {
                    var keyValuePair = expectedKeysAndValues[i];
                    if (!dictionary.TryGetValue(keyValuePair.Key, out TValue value))
                    {
                        Assert.Fail($"{i} not found");
                    }
                    else if (!keyValuePair.Value.Equals(value))
                    {
                        Assert.Fail($"Value for key {i} should be {keyValuePair.Value} not {value}.");
                    }
                }
            }
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
    }
}
