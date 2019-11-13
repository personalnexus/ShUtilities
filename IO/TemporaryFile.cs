using System;
using System.IO;

namespace ShUtilities.IO
{
    /// <summary>
    /// Creates a temporary file via <see cref="Path.GetTempFileName"/> and makes sure it is deleted using the Dispose pattern
    /// </summary>
    public class TemporaryFile : IDisposable
    {
        public TemporaryFile()
        {
            Name = Path.GetTempFileName();
        }

        public void Dispose()
        {
            File.Delete(Name);
        }

        public string Name { get; }

        //
        // Some methods from the File class for convenience
        //

        public string ReadAllText()
        {
            string result = File.ReadAllText(Name);
            return result;
        }

        public void WriteAllText(string contents)
        {
            File.WriteAllText(Name, contents);
        }
    }
}
