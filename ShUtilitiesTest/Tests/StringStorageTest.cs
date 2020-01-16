using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Text;
using System;
using System.Text;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class StringStorageTest
    {
        [TestMethod]
        public void EncodingWrong()
        {
            ExceptionUtility.Expect<ArgumentException>(() => new SingleByteStringStorage(Encoding.UTF8));
        }

        [TestMethod]
        public void Null()
        {
            ExceptionUtility.Expect<NullReferenceException>(() => new SingleByteString().ToString());
        }

        [TestMethod]
        public void Roundtrip()
        {
            var storage = new SingleByteStringStorage();
            SingleByteString hello = storage.Add("Hello");
            SingleByteString world = storage.Add("World");

            Assert.AreEqual(5, hello.Length);
            Assert.AreEqual("Hello", hello.ToString());
            Assert.AreEqual("Hello World", hello + " " + world);
        }
    }
}
