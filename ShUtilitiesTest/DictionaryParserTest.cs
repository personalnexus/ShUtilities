using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ShUtilities.Text;

namespace ShUtilitiesTest
{
    [TestClass]
    public class DictionaryParserTest
    {
        private readonly DictionaryParser _dp = new DictionaryParser(new Dictionary<string, string>
        {
            {"Key1", "Value1" },
            {"Key2", "2" },
            {"Key3", "true" },
        });

        [TestMethod]
        public void GetValue()
        {
            Assert.AreEqual("Value1", _dp.GetValue("Key1"));
            Assert.AreEqual(2, _dp.GetValue<int>("Key2", int.TryParse));
            Assert.AreEqual(true, _dp.GetValue<bool>("Key3", bool.TryParse));
        }
    }
}
