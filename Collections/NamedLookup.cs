using System;
using System.Collections.Generic;
using System.Linq;

namespace ShUtilities.Collections
{
    /// <summary>
    /// Provides a lookup from names contained in one array to values contained in a second array.
    /// </summary>
    public class NamedLookup: IDisposable
    {
        private readonly Dictionary<string, int> _indexesByName;
        
        public NamedLookup(params string[] names)
        {
            int index = 0;
            _indexesByName = names.ToDictionary(name => name, name => index++);
        }

        public void Dispose()
        {
            Values = null;
        }

        /// <summary>
        /// Sets <see cref="Values"/> and returns an <see cref="IDisposable"/> to clear <see cref="Values"/> after use. 
        /// </summary>
        /// <param name="values"></param>
        /// <remarks>I'm not sure if this is good design. It seems like kind of an abuse of the Dispose pattern, to indicate for how long Values is supposed to be valid.</remarks>
        public IDisposable SetValues(params object[] values)
        {
            Values = values;
            return this;
        }

        public object[] Values { get; set; }

        /// <summary>
        /// Gets a value, performing a name to index lookup on each call.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T GetValue<T>(string name) => GetValue<T>(_indexesByName[name]);

        public T GetValue<T>(int index)
        {
            object value = Values[index];
            T typedValue = (T)value;
            return typedValue;
        }

        /// <summary>
        /// Gets a <see cref="NamedLookupElement{T}"/> which can be used repeatedly to look up values as the <see cref="Values"/>
        /// property is updated without performing name to index lookup on each call.
        /// </summary>
        public NamedLookupElement<T> Get<T>(string name) => new NamedLookupElement<T>(this, _indexesByName[name]);

        public bool TryGet<T>(string name, out NamedLookupElement<T> element)
        {
            bool result = _indexesByName.TryGetValue(name, out int index);
            element = result ? new NamedLookupElement<T>(this, index) : default;
            return result;
        }
    }
}
