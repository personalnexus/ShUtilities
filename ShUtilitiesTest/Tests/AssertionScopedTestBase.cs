using FluentAssertions.Execution;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ShUtilitiesTest.Tests
{
    public class AssertionScopedTestBase
    {
        private AssertionScope _scope;

        [TestInitialize]
        public void Initialize() => _scope = new AssertionScope();

        [TestCleanup]
        public void Cleanup() => _scope.Dispose();
    }
}