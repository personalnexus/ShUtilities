﻿using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace ShUtilities.Interop
{
    /// <summary>
    /// Wraps an unmanaged library loaded via the p-invoked LoadLibrary into an <see cref="IDisposable"/> and allows discovery of functions from that library at runtime
    /// </summary>
    public class Library : IDisposable
    {
        private IntPtr _handle;

        public IntPtr Handle => _handle;
        public string Name { get; private set; }

        /// <summary>
        /// Loads the given library
        /// </summary>
        /// <param name="libraryName">Name of the library to load</param>
        public Library(string libraryName)
        {
            _handle = NativeMethods.LoadLibrary(libraryName);
            if (_handle == IntPtr.Zero)
            {
                throw new ArgumentException($"Cannot load library '{libraryName}'. {InteropUtility.GetLastErrorCodeAndMessage()}");
            }
            Name = libraryName;
        }

        /// <summary>
        /// Loads either the 32 or 64 bit version of a library based on the bitness of the current process
        /// </summary>
        /// <param name="libraryName32Bit">Name of the 32 bit version of the library to load</param>
        /// <param name="libraryName64Bit">Name of the 64 bit version of the library to load</param>
        public Library(string libraryName32Bit, string libraryName64Bit) : this(Environment.Is64BitProcess ? libraryName64Bit : libraryName32Bit)
        {
        }

        /// <summary>
        /// Frees the unmanaged library represented by this object
        /// </summary>
        public void Dispose()
        {
            InteropUtility.ClearPointer(ref _handle, NativeMethods.FreeLibrary);
            Name = null;
        }

        /// <summary>
        /// Gets a delegate for an unmanaged function of the given name
        /// </summary>
        /// <typeparam name="TDelegate">The type of delegate to which to convert the function pointer</typeparam>
        /// <param name="functionName">The name of the function</param>
        public TDelegate GetDelegate<TDelegate>(string functionName)
        {
            if (!TryGetFunctionPointer(functionName, out IntPtr functionPointer))
            {
                throw new ArgumentException($"Cannot load function '{functionName}' from library '{Name}'. {InteropUtility.GetLastErrorCodeAndMessage()}");
            }
            TDelegate result = Marshal.GetDelegateForFunctionPointer<TDelegate>(functionPointer);
            return result;
        }

        /// <summary>
        /// Gets a delegate for an unmanaged function using the name of the delegate type as the function name
        /// </summary>
        /// <typeparam name="TDelegate">The type of delegate to which to convert the function pointer</typeparam>
        public TDelegate GetDelegate<TDelegate>()
        {
            string functionName = typeof(TDelegate).Name;
            return GetDelegate<TDelegate>(functionName);
        }

        /// <summary>
        /// Tries to get a function pointer for an unmanaged function of the given name
        /// </summary>
        /// <param name="functionName">The name of the function</param>
        /// <returns>True, if such a function exists, otherwise false</returns>
        public bool TryGetFunctionPointer(string functionName, out IntPtr functionPointer)
        {
            functionPointer = NativeMethods.GetProcAddress(_handle, functionName);
            bool result = functionPointer != IntPtr.Zero;
            return result;
        }
    }
}
