using System;
using System.ComponentModel;

namespace ShUtilities.Interop
{
    internal static class InteropUtility
    {
        /// <summary>
        /// Use this helper method e.g. in Dispose to check if a pointer is non-zero and if so, call a clean-up method on it
        /// </summary>
        public static void ClearPointer(ref IntPtr pointer, Action<IntPtr> clearMethod)
        {
            if (pointer != IntPtr.Zero)
            {
                // Set pointer to zero before calling the clear method, so we aren't left with an invalid pointer in case that method fails
                IntPtr localPointer = pointer;
                pointer = IntPtr.Zero;
                clearMethod(localPointer);
            }
        }

        public static string GetLastErrorCodeAndMessage()
        {
            var win32Error = new Win32Exception();
            string result = $"Error code: {win32Error.NativeErrorCode}. Error message: {win32Error.Message}.";
            return result;
        }
    }
}
