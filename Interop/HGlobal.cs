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
            _pointer = Marshal.AllocHGlobal(byteCount);
        }

        /// <summary>
        /// Frees the unmanaged memory via <see cref="Marshal.FreeHGlobal"/>
        /// </summary>
        public virtual void Dispose()
        {
            InteropUtility.ClearPointer(ref _pointer, Marshal.FreeHGlobal);
        }

        /// <summary>
        /// Pointer to the unmanaged memory
        /// </summary>
        public IntPtr Pointer => _pointer;

        private IntPtr _pointer;
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
            Marshal.StructureToPtr<T>(structure, Pointer, false);
        }

        /// <summary>
        /// Destroys the structure via <see cref="Marshal.DestroyStructure{T}"/> before freeing the unmanaged memory via <see cref="Marshal.FreeHGlobal"/>
        /// </summary>
        public override void Dispose()
        {
            if (Pointer != IntPtr.Zero)
            {
                Marshal.DestroyStructure<T>(Pointer);
                base.Dispose();
            }
        }

        /// <summary>
        /// Replaces the unmanaged memory with a copy of a new structure after destroying the old one.
        /// </summary>
        /// <param name="newStructure"></param>
        public void ReplaceStructure(T newStructure)
        {
            Marshal.StructureToPtr<T>(newStructure, Pointer, true);
        }

        /// <summary>
        /// Returns a copy of the structure from unmanaged memory via <see cref="Marshal.PtrToStructure{T}(IntPtr)"/>
        /// </summary>
        /// <returns></returns>
        public T ToStructure()
        {
            T result = Marshal.PtrToStructure<T>(Pointer);
            return result;
        }
    }
}
