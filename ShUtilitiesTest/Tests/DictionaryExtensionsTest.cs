using ShUtilities.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class DictionaryExtensionsTest
    {
        [TestMethod]
        public void Diff()
        {
            var original = new Dictionary<string, double>
            {
                ["1"] = 1.0,
                ["2"] = 2.0,
                ["3"] = 3.0,
            };
            var other = new Dictionary<string, double>
            {
                ["1"] = 1.0,
                ["2"] = 2.1,
                ["4"] = 4.0,
            };

            Assert.IsFalse(original.Diff(original, out _));

            Assert.IsTrue(original.Diff(other, out DictionaryDiff<string, double> diff));
            Check("1", 1.0, diff.Unchanged);
            Check("3", 3.0, diff.Removed);
            Check("4", 4.0, diff.Added);

            Assert.AreEqual(1, diff.Changed.Count);
            Assert.AreEqual("2", diff.Changed[0].Key);
            Assert.AreEqual(2.0, diff.Changed[0].OldValue);
            Assert.AreEqual(2.1, diff.Changed[0].NewValue);

            Assert.AreEqual(
@$"Added: 1 
[4, 4]

Removed: 1 
[3, 3]

Changed Values: 1 
[2, 2 -> {2.1}]

Unchanged: 1 
[1, 1]", diff.GetDescription());

            static void Check(string expectedKey, double expectedValue, IReadOnlyList<KeyValuePair<string, double>> list)
            {
                Assert.AreEqual(1, list.Count);
                Assert.AreEqual(expectedKey, list[0].Key);
                Assert.AreEqual(expectedValue, list[0].Value);
            }
        }

        [TestMethod]
        public void GetOrAdd()
        {
            int CreateValueCalled = 0;
            string CreateValue(string key)
            {
                CreateValueCalled++;
                return "Value" + key;
            }

            string CreateValueFromState(string key, string state)
            {
                CreateValueCalled++;
                return state + key;
            }

            var dictionary = new Dictionary<string, string>();
            Assert.AreEqual("Value1", dictionary.GetOrAdd("1", CreateValue));
            Assert.AreEqual("Value1", dictionary.GetOrAdd("1", CreateValue));
            Assert.AreEqual("StateX", dictionary.GetOrAdd("X", CreateValueFromState, "State"));

            Assert.AreEqual(2, CreateValueCalled);
        }

        [TestMethod]
        public void GetValueOrDefault()
        {
            var dictionary = new Dictionary<int, string>
            {
                [1] = "Number One",
                [2] = "Close",
                [3] = "Nevermind"
            };
            Assert.AreEqual("Number One", dictionary.GetValueOrDefault(1, "X"));
            Assert.AreEqual("X", dictionary.GetValueOrDefault(4, "X"));
            Assert.IsNull(dictionary.GetValueOrDefault(9));
        }

        [TestMethod]
        public void Remove()
        {
            var dictionary = new Dictionary<int, string>
            {
                [1] = "Number One",
                [2] = "Close",
                [3] = "Nevermind"
            };
            Assert.IsTrue(dictionary.Remove(2, out string text));
            Assert.AreEqual("Close", text);
            Assert.IsFalse(dictionary.ContainsKey(2));
            Assert.IsFalse(dictionary.Remove(2, out text));
            Assert.IsNull(text);
        }
    }
}
