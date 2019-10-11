using System.IO;

namespace ShUtilities.IO
{
    /// <summary>
    /// Basic interface describing serialization e.g. XML, JSON, CSV...
    /// </summary>
    public interface ISerializer<T>
    {
        T Deserialize(Stream input);
        void Serialize(T input, Stream output);
    }
}
