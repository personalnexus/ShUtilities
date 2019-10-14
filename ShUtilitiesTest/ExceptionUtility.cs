using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ShUtilitiesTest
{
    public static class ExceptionUtility
    {
        /// <summary>
        /// Similar in purpose to the [ExpectedException] attribute, this helper method allows making sure a specific piece of code throws the expected exception
        /// </summary>
        public static void Expect<TException>(Action testMethod)
        {
            Expect(testMethod, caughtException => Assert.IsInstanceOfType(caughtException, typeof(TException)));
        }

        /// <summary>
        /// Similar in purpose to the [ExpectedException] attribute, this helper method allows verifying the exception thrown by a specific piece of code
        /// </summary>
        public static void Expect(this Action testMethod, Action<Exception> exceptionVerifier)
        {
            Exception caughtException = null;
            try
            {
                testMethod();
            }
            catch (Exception exception)
            {
                caughtException = exception;
            }
            Assert.IsNotNull(caughtException, "Expected an exception, but none was raised.");
            exceptionVerifier(caughtException);
        }
    }
}
