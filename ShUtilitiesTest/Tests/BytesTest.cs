using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Common;
using System;
using System.Globalization;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class BytesTest
    {
        [TestMethod]
        public void Cloning()
        {
            var megaByte = Bytes.FromMega(1);
            var anotherMegaByte = megaByte.Clone();
            Assert.AreEqual(megaByte, anotherMegaByte);
        }    
        
        [TestMethod]
        public void Comparison()
        {
            var megaByte = Bytes.FromMega(1);
            var anotherMegaByte = Bytes.FromMega(1);
            var gigaByte = Bytes.FromGiga(1);
            Assert.AreEqual(-1, megaByte.CompareTo(gigaByte));
            Assert.AreEqual(0, megaByte.CompareTo(anotherMegaByte));
            Assert.AreEqual(1, gigaByte.CompareTo(megaByte));
            Assert.IsTrue(megaByte < gigaByte);
            Assert.IsFalse(megaByte > gigaByte);
            Assert.IsTrue(megaByte <= gigaByte);
            Assert.IsFalse(megaByte >= gigaByte);
            Assert.IsTrue(megaByte <= anotherMegaByte);
            Assert.IsTrue(megaByte == anotherMegaByte);
            Assert.IsTrue(megaByte >= anotherMegaByte);
            Assert.IsTrue(megaByte != gigaByte);
            Assert.IsFalse(megaByte == gigaByte);
        }

        [TestMethod]
        public void FormatBytes()
        {
            var bytes = new Bytes(615);
            Assert.AreEqual("615B", bytes.ToString());
            Assert.AreEqual(615.0.ToString("F1") + "B", bytes.ToString("F1"));
            Assert.AreEqual<int>(615, bytes);
            Assert.AreEqual<ulong>(615, bytes);
            Assert.AreEqual<string>("615B", bytes);
        }

        [TestMethod]
        public void FormatMegaBytes()
        {
            var megaBytes = Bytes.FromMega(2.345);
            Assert.AreEqual(2.35 + "MB", megaBytes.ToString());
            Assert.AreEqual("FileSize=" + 2.35 + "MB", $"FileSize={megaBytes}");
        }

        [TestMethod]
        public void TotalGigaBytes()
        {
            var bytes = Bytes.FromGiga(2.345) + Bytes.FromMega(10240);
            Assert.AreEqual(12.345, Math.Round(bytes.TotalGiga, 5));
        }

        [TestMethod]
        public void Operators()
        {
            var twoMB = Bytes.FromMega(2);
            Assert.AreEqual("4MB", (2 * twoMB).ToString());
            Assert.AreEqual("4MB", (twoMB + twoMB).ToString());
            Assert.AreEqual(Bytes.FromMega(4), 2 * twoMB);
        }

        [TestMethod]
        public void Overflow()
        {
            var exaBytes = Bytes.FromExa(4.12);
            Assert.AreEqual(4.12 + "EB", exaBytes.ToString());
            ExceptionUtility.Expect<OverflowException>(() => Bytes.FromExa(418));
        }
    }
}
