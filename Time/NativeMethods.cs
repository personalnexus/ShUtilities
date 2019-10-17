using System.Runtime.InteropServices;

namespace ShUtilities.Time
{
    internal static class NativeMethods
    {
        [DllImport("Kernel32.dll")]
        public static extern bool QueryPerformanceCounter(out long ticks);
    }
}
