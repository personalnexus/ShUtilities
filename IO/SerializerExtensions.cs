using System.IO;
using System.Text;

namespace ShUtilities.IO
{
    /// <summary>
    /// Extension methods for the basic <see cref="ISerializer"/> interface.
    /// </summary>
    public static class SerializerExtensions
    {
        // File path

        public static T DeserializeFile<T>(this ISerializer<T> serializer, string inputPath, FileMode mode = FileMode.OpenOrCreate, FileAccess access = FileAccess.Write, FileShare share = FileShare.Read)
        {
            T result;
            using (var inputStream = new FileStream(inputPath, mode, access, share))
            {
                result = serializer.Deserialize(inputStream);
            }
            return result;
        }

        public static void SerializeFile<T>(this ISerializer<T> serializer, T input, string outputPath, FileMode mode = FileMode.OpenOrCreate, FileAccess access = FileAccess.Write, FileShare share = FileShare.Read)
        {
            using var outputStream = new FileStream(outputPath, mode, access, share);
            serializer.Serialize(input, outputStream);
        }

        // String

        public static T DeserializeString<T>(this ISerializer<T> serializer, string input)
        {
            return DeserializeString(serializer, input, Encoding.UTF8);
        }

        public static T DeserializeString<T>(this ISerializer<T> serializer, string input, Encoding encoding)
        {
            T result;
            using (var inputStream = new MemoryStream(encoding.GetBytes(input)))
            {
                result = serializer.Deserialize(inputStream);
            }
            return result;
        }

        public static string SerializeString<T>(this ISerializer<T> serializer, T input)
        {
            return SerializeString(serializer, input, Encoding.UTF8);
        }

        public static string SerializeString<T>(this ISerializer<T> serializer, T input, Encoding encoding)
        {
            string result;
            using (var outputStream = new MemoryStream())
            {
                serializer.Serialize(input, outputStream);
                outputStream.Position = 0;
                using var reader = new StreamReader(outputStream, encoding);
                result = reader.ReadToEnd();
            }
            return result;
        }
    }
}
