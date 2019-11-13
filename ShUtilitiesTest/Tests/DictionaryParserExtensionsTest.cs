using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ShUtilities.Text;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class DictionaryParserExtensionsTest
    {
        private readonly Dictionary<string, string> _dictionary = new Dictionary<string, string>
        {
            {"Key1", "Value1" },
            {"Key2", "2" },
            {"Key3", "true" },
        };

        [TestMethod]
        public void GetValue()
        {
            Assert.AreEqual("Value1", _dictionary.GetValue("Key1"));
            Assert.AreEqual(2, _dictionary.GetValue<string, string, int>("Key2", int.TryParse));
            Assert.AreEqual(true, _dictionary.GetValue<string, string, bool>("Key3", bool.TryParse));
        }

        [TestMethod]
        public void TryGetValue()
        {
            Assert.IsTrue(_dictionary.TryGetValue("Key2", out int value2, int.TryParse));
            Assert.AreEqual(2, value2);
        }
    }
}
