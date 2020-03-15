using System.Runtime.InteropServices;

namespace ShUtilities.Time
{
    internal static class NativeMethods
    {
        static NativeMethods()
        {
            QueryPerformanceFrequency(out long ticksPerSecond);
            TicksPerSecond = ticksPerSecond;
        }

        // Helper property, because the result of QueryPerformanceFrequency does not change
        internal static readonly long TicksPerSecond;

        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern bool QueryPerformanceCounter(out long ticks);

        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern bool QueryPerformanceFrequency(out long ticksPerSecond);
    }
}
