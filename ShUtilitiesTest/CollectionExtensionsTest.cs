using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ShUtilitiesTest
{
    [TestClass]
    public class CollectionExtensionsTest
    {
        [TestMethod]
        public void ToDelimitedString()
        {
            Assert.AreEqual("1,2,3,4,5", Enumerable.Range(1, 5).ToDelimitedString(","));
        }

        [TestMethod]
        public void GetOrAdd()
        {
            var dictionary = new Dictionary<string, string>();
            Assert.AreEqual("Value1", dictionary.GetOrAdd("1", CreateValue));
            Assert.AreEqual("Value1", dictionary.GetOrAdd("1", CreateValue));
            Assert.AreEqual("StateX", dictionary.GetOrAdd("X", CreateValue, "State"));
        }

        private string CreateValue(string key)
        {
            return "Value" + key;
        }

        private string CreateValue(string key, string state)
        {
            return state + key;
        }
    }
}
