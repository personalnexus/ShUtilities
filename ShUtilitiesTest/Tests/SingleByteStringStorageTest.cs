using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Text;
using System;
using System.Linq;
using System.Text;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class SingleByteStringStorageTest
    {
        [TestMethod]
        public void AddGet()
        {
            var storage = new SingleByteStringStorage();
            SingleByteString hello = storage.Add("Hello");
            Assert.AreEqual(1, storage.Count);
            SingleByteString world = storage.Add("World");
            Assert.AreEqual(2, storage.Count);

            Assert.AreEqual(5, hello.Length);
            Assert.AreEqual("Hello", hello.ToString());
            Assert.AreEqual("Hello World", hello + " " + world);

            CollectionAssert.AreEqual(new[] { "Hello", "World" }, storage.GetStrings().ToArray());
        }

        [TestMethod]
        public void AddRemove()
        {
            var storage = new SingleByteStringStorage();
            SingleByteString hello = storage.Add("Hello");

            Assert.AreEqual(9, storage.TotalUsedBytes); // +4 for header
            Assert.IsTrue(storage.Remove(hello));
            Assert.AreEqual(0, storage.Count);

            Assert.AreEqual(9, storage.TotalUsedBytes); // Unchanged after removal
            Assert.IsFalse(storage.Remove(hello));
            Assert.AreEqual(0, storage.Count);

            CollectionAssert.AreEqual(new string[0], storage.GetStrings().ToArray());
        }

        [TestMethod]
        public void Combine()
        {
            var storage = new SingleByteStringStorage();
            SingleByteString hello = storage.Add("Hello");
            SingleByteString world = storage.Add("World");
            SingleByteString _blank_ = storage.Add(" ");
            Assert.AreEqual("Hello World", hello + _blank_ + world);
            Assert.AreEqual<string>("Hello", hello);
        }

        [TestMethod]
        public void Equality()
        {
            var storage = new SingleByteStringStorage();
            SingleByteString hello1 = storage.Add("Hello");
            SingleByteString hello2 = storage.Add("Hello");
            SingleByteString worlds = storage.Add("Worlds");
            Assert.AreEqual(3, storage.Count);

            Assert.IsTrue(hello1.Equals(hello2));
            Assert.IsTrue(hello1 == hello2);
            Assert.IsFalse(hello1 != hello2);
            Assert.AreEqual(hello1.GetHashCode(), hello2.GetHashCode());

            Assert.IsFalse(hello1.Equals(worlds));
            Assert.IsFalse(hello1 == worlds);
            Assert.IsTrue(hello1 != worlds);
            Assert.AreNotEqual(hello1.GetHashCode(), worlds.GetHashCode());
        }

        [TestMethod]
        public void Null()
        {
            var storage = new SingleByteStringStorage();
            var sbs = new SingleByteString();
            ExceptionUtility.Expect<NullReferenceException>(() => storage.Remove(new SingleByteString()));
            ExceptionUtility.Expect<NullReferenceException>(() => sbs.ToString());
            ExceptionUtility.Expect<NullReferenceException>(() => Assert.AreEqual(0, sbs.Length));
        }

        [TestMethod]
        public void Options()
        {
            // Small GrowthIncrement forces a re-allocation
            var storage = new SingleByteStringStorage(new SingleByteStringStorageOptions { GrowthIncrement = 2 });
            Assert.IsNotNull(storage?.Options?.Encoding);
            ExceptionUtility.Expect<ArgumentException>(() => new SingleByteStringStorage(new SingleByteStringStorageOptions(Encoding.UTF8)));
            SingleByteString sbs = storage.Add("A string that is longer than the GrowthIncrement");
            Assert.AreEqual(sbs.Length + 4, storage.TotalUsedBytes); // +4 for header
        }
    }
}
