﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Interop;
using System;

namespace ShUtilitiesTest
{
    [TestClass]
    public class LibraryTest
    {
        [TestMethod]
        public void LoadLibrary()
        {
            using (var library = new Library("kernel32.dll", "kernel64.dll"))
            {
                Assert.AreEqual("kernel32.dll", library.Name);
            }
        }

        [TestMethod]
        public void TryGetFunctionPointer()
        {
            using (var library = new Library("kernel32.dll"))
            {
                Assert.IsTrue(library.TryGetFunctionPointer("FreeLibrary", out IntPtr pointer));
            }
        }

        [TestMethod]
        public void GetDelegate()
        {
            using (var library = new Library("kernel32.dll"))
            {
                var getLastError = library.GetDelegate<GetLastError>("GetLastError");
                Assert.IsNotNull(getLastError);
                getLastError();
            }
        }

        [TestMethod]
        public void GetDelegateWithoutFunctionName()
        {
            using (var library = new Library("kernel32.dll"))
            {
                var getLastError = library.GetDelegate<GetLastError>();
                Assert.IsNotNull(getLastError);
                getLastError();
            }
        }

        private delegate int GetLastError();
    }
}
