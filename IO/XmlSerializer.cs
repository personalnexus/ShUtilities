using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ShUtilities.IO
{
    /// <summary>
    /// Implementation of <see cref="ISerializer{T}"/> for XML
    /// </summary>
    public class XmlSerializer<T> : ISerializer<T>
    {
        private XmlSerializer _serializer = new XmlSerializer(typeof(T));

        public XmlReaderSettings ReaderSettings { get; } = new XmlReaderSettings();
        public XmlWriterSettings WriterSettings { get; } = new XmlWriterSettings();

        public T Deserialize(Stream input)
        {
            T result;
            using (XmlReader reader = XmlReader.Create(input, ReaderSettings))
            {
                result = (T)_serializer.Deserialize(reader);
            }
            return result;
        }

        public void Serialize(T input, Stream output)
        {
            using (XmlWriter writer = XmlWriter.Create(output, WriterSettings))
            {
                _serializer.Serialize(writer, input);
            }
        }
    }
}
