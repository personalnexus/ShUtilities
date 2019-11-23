using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Common;
using System;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class BytesTest
    {
        [TestMethod]
        public void FormatBytes()
        {
            var bytes = new Bytes(615);
            Assert.AreEqual("615B", bytes.ToString());
            Assert.AreEqual<ulong>(615, bytes.TotalBytes);
        }

        [TestMethod]
        public void FormatMegaBytes()
        {
            var megaBytes = Bytes.FromMega(2.345);
            Assert.AreEqual("2.35MB", megaBytes.ToString());
            Assert.AreEqual("FileSize=2.35MB", $"FileSize={megaBytes}");
        }

        [TestMethod]
        public void TotalGigaBytes()
        {
            var bytes = Bytes.FromGiga(12.345);
            Assert.AreEqual(12.345, Math.Round(bytes.TotalGiga, 3));
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
            Assert.AreEqual("4.12EB", exaBytes.ToString());
            ExceptionUtility.Expect<OverflowException>(() => Bytes.FromExa(418));
        }
    }
}
