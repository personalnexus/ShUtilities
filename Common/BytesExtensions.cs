using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShUtilities.Common
{
    /// <summary>
    /// Extension methods for classes exposing memory sizes so you don't have to convert these sizes into <see cref="Bytes"/> instances manually.
    /// </summary>
    public static class BytesExtensions
    {
        //
        // Process
        //
        public static Bytes NonpagedSystemMemorySizeBytes(this Process process) => new Bytes(process.NonpagedSystemMemorySize64);
        public static Bytes PagedMemorySizeBytes(this Process process) => new Bytes(process.PagedMemorySize64);
        public static Bytes PagedSystemMemorySizeBytes(this Process process) => new Bytes(process.PagedSystemMemorySize64);
        public static Bytes PeakPagedMemorySizeBytes(this Process process) => new Bytes(process.PeakPagedMemorySize64);
        public static Bytes PeakVirtualMemorySizeBytes(this Process process) => new Bytes(process.PeakVirtualMemorySize64);
        public static Bytes PeakWorkingSetBytes(this Process process) => new Bytes(process.PeakWorkingSet64);
        public static Bytes PrivateMemorySizeBytes(this Process process) => new Bytes(process.PrivateMemorySize64);
        public static Bytes VirtualMemorySizeBytes(this Process process) => new Bytes(process.VirtualMemorySize64);
        public static Bytes WorkingSetBytes(this Process process) => new Bytes(process.WorkingSet64);
    }
}
