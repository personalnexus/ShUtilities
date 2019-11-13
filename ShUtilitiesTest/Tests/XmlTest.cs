using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.IO;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class XmlTest
    {
        [TestMethod]
        public void PrettyPrint()
        {
            using (var inputFile = new TemporaryFile())
            using (var outputFile = new TemporaryFile())
            {
                inputFile.WriteAllText("<?xml   version=\"1.0\"   encoding=\"utf-8\"?><xml><node attr=\"value\"/></xml>");
                XmlUtility.PrettyPrint(inputFile.Name, outputFile.Name);
                Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<xml>\r\n  <node attr=\"value\" />\r\n</xml>", outputFile.ReadAllText());
            }
        }
    }
}
