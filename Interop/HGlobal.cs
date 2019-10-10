using System;
using System.Runtime.InteropServices;

namespace ShUtilities.Interop
{
    /// <summary>
    /// Wraps allocation and de-allocation of a number of bytes of unmanaged memory into an <see cref="IDisposable"/> to be used in a using-statement
    /// </summary>
    public class HGlobal: IDisposable
    {
        /// <summary>
        /// Allocates the given number of bytes of unmanaged memory via <see cref="Marshal.AllocHGlobal"/>
        /// </summary>
        /// <param name="byteCount">Number of bytes to allocate</param>
        public HGlobal(int byteCount)
        {
            Pointer = Marshal.AllocHGlobal(byteCount);
        }

        /// <summary>
        /// Frees the unmanaged memory via <see cref="Marshal.FreeHGlobal"/>
        /// </summary>
        public void Dispose()
        {
            if (Pointer != IntPtr.Zero)
            {
                IntPtr pointer = Pointer;
                Pointer = IntPtr.Zero;
                Marshal.FreeHGlobal(pointer);
            }
        }

        /// <summary>
        /// Pointer to the unmanaged memory
        /// </summary>
        public IntPtr Pointer { get; private set; }
    }

    /// <summary>
    /// Wraps allocation and de-allocation of memory for a structure into an <see cref="IDisposable"/> to be used in a using-statement
    /// </summary>
    public class HGlobal<T>: HGlobal
    {
        /// <summary>
        /// Allocates memory for the given structure (number of bytes determined by <see cref="Marshal.SizeOf{T}"/>) and
        /// copies the structure to it via <see cref="Marshal.StructureToPtr"/>
        /// </summary>
        /// <param name="structure">The structure or formatted class to be copied to unmanaged memory</param>
        public HGlobal(T structure): base(Marshal.SizeOf<T>(structure))
        {
            Marshal.StructureToPtr(structure, Pointer, false);
        }

        public T ToStructure()
        {
            T result = Marshal.PtrToStructure<T>(Pointer);
            return result;
        }
    }
}
