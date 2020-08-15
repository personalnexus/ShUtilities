using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;

namespace ShUtilities.IO
{
    /// <summary>
    /// Exposes the contents of files as string properties of a dynamic object
    /// </summary>
    public class FileRepository: DynamicObject
    {
        public FileRepository(string filePattern)
        {
            Reload(filePattern);
        }

        private Dictionary<string, string> _contentsByName;
        
        public void Reload(string filePattern)
        {
            string path = Path.GetDirectoryName(filePattern) ?? "";
            string searchPattern = Path.GetFileName(filePattern) ?? "*";
            _contentsByName = Directory.EnumerateFiles(path, searchPattern).ToDictionary(x => Path.GetFileNameWithoutExtension(x), x => File.ReadAllText(x), StringComparer.OrdinalIgnoreCase);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object value)
        {
            bool result = TryFileContents(binder.Name, out string valueString);
            value = valueString;
            return result;
        }

        protected bool TryFileContents(string fileName, out string contents) => _contentsByName.TryGetValue(fileName, out contents);
    }
}
