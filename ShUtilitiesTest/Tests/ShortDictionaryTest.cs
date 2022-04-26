using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Collections;
using System;
using System.Collections.Generic;
using static ShUtilitiesTest.ExceptionUtility;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class ShortDictionaryTest
    {
        [TestMethod]
        public void Add_Get_Remove_Count()
        {
            var dictionary = new ShortDictionary<string>();
            Assert.AreEqual(0, dictionary.Count);

            dictionary.Add(1, "Value1a");
            Assert.AreEqual("Value1a", dictionary[1]);
            Assert.AreEqual(1, dictionary.Count);

            Assert.IsTrue(dictionary.Remove(1));
            Assert.AreEqual(0, dictionary.Count);

            dictionary[1] = "Value1b";
            Assert.AreEqual("Value1b", dictionary[1]);
            Assert.AreEqual(1, dictionary.Count);

            dictionary[1] = "Value1c";
            Assert.AreEqual(1, dictionary.Count);
            Assert.AreEqual("Value1c", dictionary[1]);

            Assert.IsFalse(dictionary.Remove(2));
            Assert.AreEqual(1, dictionary.Count);
        }

        [TestMethod]
        public void Add_Get_EdgeCases()
        {
            var dictionary = new ShortDictionary<string>();
            dictionary.Add(short.MinValue, "MinValue");
            dictionary.Add(-1, "NegativeOne");
            dictionary.Add(short.MaxValue, "MaxValue");

            Assert.AreEqual(3, dictionary.Count);
            Assert.AreEqual("MinValue", dictionary[short.MinValue]);
            Assert.AreEqual("NegativeOne", dictionary[-1]);
            Assert.AreEqual("MaxValue", dictionary[short.MaxValue]);
        }

        [TestMethod]
        public void ReadOnly()
        {
            var dictionary = new ShortDictionary<string>
            {
                {1, "Value1" },
                {2, "Value2" },
            };
            dictionary.IsReadOnly = true;
            Expect<InvalidOperationException>(() => dictionary[3] = "Value3");
            Expect<InvalidOperationException>(() => dictionary.Add(4, "Value4"));
            Expect<InvalidOperationException>(() => dictionary.GetOrAdd<short, string>(5, x => "Value5"));

            dictionary.IsReadOnly = false;
            dictionary[3] = "Value3";
            Assert.AreEqual("Value3", dictionary[3]);
        }

        [TestMethod]
        public void Clear_ContainsKey()
        {
            var dictionary = new ShortDictionary<string>
            {
                {0, "Value0" },
                {1, "Value1" },
                {2, "Value2" },
                {255, "Value255" },
            };
            Assert.IsTrue(dictionary.ContainsKey(0));
            Assert.IsTrue(dictionary.ContainsKey(1));
            Assert.IsTrue(dictionary.ContainsKey(2));
            Assert.IsFalse(dictionary.ContainsKey(3));
            Assert.IsTrue(dictionary.ContainsKey(255));
            Assert.AreEqual(4, dictionary.Count);

            dictionary.Clear();

            Assert.AreEqual(0, dictionary.Count);
            Assert.IsFalse(dictionary.ContainsKey(0));
            Assert.IsFalse(dictionary.ContainsKey(1));
            Assert.IsFalse(dictionary.ContainsKey(2));
            Assert.IsFalse(dictionary.ContainsKey(3));
            Assert.IsFalse(dictionary.ContainsKey(255));
        }
    }
}
