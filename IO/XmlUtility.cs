using System.Xml;

namespace ShUtilities.IO
{
    /// <summary>
    /// Helper methods for working with XML
    /// </summary>
    public static class XmlUtility
    {
        /// <summary>
        /// Takes an XML document from an input file and writes it pretty printed into the output file
        /// </summary>
        public static void PrettyPrint(string inputFilename, string outputFilename)
        {
            var writerSettings = new XmlWriterSettings { Indent = true };
            PrettyPrint(inputFilename, outputFilename, writerSettings);
        }

        /// <summary>
        /// Takes an XML document from an input file and writes it pretty printed into the output file using the given settings to fine-tune pretty print settings
        /// </summary>
        private static void PrettyPrint(string inputFilename, string outputFilename, XmlWriterSettings writerSettings)
        {
            using var writer = XmlWriter.Create(outputFilename, writerSettings);
            var document = new XmlDocument();
            document.Load(inputFilename);
            document.Save(writer);
        }
    }
}
