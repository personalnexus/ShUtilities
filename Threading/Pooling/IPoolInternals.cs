using System.Collections.Generic;

namespace ShUtilities.Threading.Pooling
{
    /// <summary>
    /// Extended information about the internals of a pool for diagnostic purposes
    /// </summary>
    internal interface IPoolDiagnostics<T>
    {
        IEnumerable<T> GetObjects();
        int Count { get; }
    }
}