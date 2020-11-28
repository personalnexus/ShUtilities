using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Collections;
using System.Collections.Generic;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class CollectionExtensionsTest
    {
        public record UselessRecord();

        [TestMethod]
        public void AddNew()
        {
            var list = new List<UselessRecord>();
            object item = list.AddNew();
            Assert.IsNotNull(item);
            Assert.AreEqual(1, list.Count);
            Assert.AreSame(item, list[0]);
        }
    }
}
