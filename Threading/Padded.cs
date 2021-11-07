using System.Runtime.InteropServices;

namespace ShUtilities.Threading
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Padded<T>
    {
        //
        // Padding to avoid false sharing https://mechanical-sympathy.blogspot.com/2011/07/false-sharing.html
        //

        #pragma warning disable CS0169
        private long _padding1, _padding2, _padding3, _padding4, _padding5, _padding6, _padding7;
        #pragma warning restore CS0169

        public T Value;
        
        #pragma warning disable CS0169
        private long _padding8, _padding9, _padding10, _padding11, _padding12, _padding13, _padding14;
        #pragma warning restore CS0169
    }
}