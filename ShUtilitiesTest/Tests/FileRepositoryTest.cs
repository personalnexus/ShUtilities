using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.IO;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class FileRepositoryTest
    {
        [TestMethod]
        public void LoadFileRepository()
        {
            dynamic queries = new FileRepository(@"TestData\*.sql");
            string selectQuery = queries.Select;
            TestDataUtility.AreEqual(selectQuery, ".sql", "select");
        }
    }
}
