using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.IO;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace ShUtilitiesTest
{
    public static class TestDataUtility
    {
        /// <summary>
        /// Compare a given sting against the contents of a file with test data and show a diff tool when they are not equal
        /// </summary>
        public static void AreEqual(string actual, string fileExtension, [CallerMemberName] string fileName = null)
        {
            string expectedFileName = Path.Combine("TestData", fileName + fileExtension);
            string expected = File.ReadAllText(expectedFileName);
            try
            {
                Assert.AreEqual(expected, actual);
            }
            catch (AssertFailedException)
            {
                // Show diff tool when two strings aren't equal
                using var actualFile = new TemporaryFile(actual, fileExtension);
                using Process diff = Process.Start(DiffExecutable, string.Format(DiffArguments, actualFile.Name, expectedFileName));
                diff.WaitForExit();
                throw;
            }
        }

        private static string DiffExecutable = @"Code.exe";
        private static string DiffArguments =  "-d {0} {1}";
    }
}
