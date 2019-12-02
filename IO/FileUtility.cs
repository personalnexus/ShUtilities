using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ShUtilities.IO
{
    public static class FileUtility
    {
        /// <summary>
        /// Reads the lines of a UTF-8 encoded file one by one using the given modes for the file stream.
        /// </summary>
        /// <param name="path">The file to read</param>
        /// <param name="mode">How to open or create the file</param>
        /// <param name="access">How the file can be accessed by the FileStream object</param>
        /// <param name="share">How the file will be shared by processes</param>
        /// <returns>Lazily populated <see cref="IEnumerable{string}"/> containing each line</returns>
        public static IEnumerable<string> ReadLines(string path, FileMode mode = FileMode.Open, FileAccess access = FileAccess.Read, FileShare share = FileShare.Read)
        {
            return ReadLines(path, Encoding.UTF8, mode, access, share);
        }

        /// <summary>
        /// Reads the lines of a file one by one using the given modes for the file stream.
        /// </summary>
        /// <param name="path">The file to read</param>
        /// <param name="encoding">The file's encoding</param>
        /// <param name="mode">How to open or create the file</param>
        /// <param name="access">How the file can be accessed by the FileStream object</param>
        /// <param name="share">How the file will be shared by processes</param>
        /// <returns>Lazily populated <see cref="IEnumerable{string}"/> containing each line</returns>
        public static IEnumerable<string> ReadLines(string path, Encoding encoding, FileMode mode = FileMode.Open, FileAccess access = FileAccess.Read, FileShare share = FileShare.Read)
        {
            using var fileStream = new FileStream(path, mode, access, share);
            using var reader = new StreamReader(fileStream, encoding);
            string line = reader.ReadLine();
            while (line != null)
            {
                yield return line;
                line = reader.ReadLine();
            }
        }
    }
}
