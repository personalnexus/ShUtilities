using System.IO;
using Newtonsoft.Json;

namespace ShUtilities.IO
{
    /// <summary>
    /// Implementation of <see cref="ISerializer{T}"/> for JSON
    /// </summary>
    public class JsonSerializer<T> : ISerializer<T>
    {
        private JsonSerializer _serializer = new JsonSerializer();

        public JsonSerializer Settings => _serializer;

        public T Deserialize(Stream input)
        {
            T result;
            using (var inputStreamReader = new StreamReader(input))
            {
                using (JsonReader reader = new JsonTextReader(inputStreamReader))
                {
                    result = _serializer.Deserialize<T>(reader);
                }
            }
            return result;
        }

        public void Serialize(T input, Stream output)
        {
            using (var outputStreamWriter = new StreamWriter(output))
            {
                using (JsonWriter writer = new JsonTextWriter(outputStreamWriter))
                {
                    _serializer.Serialize(writer, input);
                }
            }
        }
    }
}
