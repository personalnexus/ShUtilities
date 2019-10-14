using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Collections;

namespace ShUtilitiesTest
{
    [TestClass]
    public class ByteDictionaryTest
    {
        [TestMethod]
        public void Add_Get_Remove_Count()
        {
            var dictionary = new ByteDictionary<string>();
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
        }
    }
}
