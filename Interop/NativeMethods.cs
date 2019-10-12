using System;
using System.Runtime.InteropServices;

namespace ShUtilities.Interop
{
    internal static class NativeMethods
    {
        [DllImport("kernel32.dll")]
        public static extern void FreeLibrary(IntPtr libraryHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr libraryHandle, string functionName);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadLibrary(string libraryName);
    }
}
