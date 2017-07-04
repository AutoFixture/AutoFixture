using System;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public static class AssertEx
    {
        public static void DoesNotThrow(Action testCode)
        {
            Exception actual = Record.Exception(testCode);
            if (actual != null)
                throw new DoesNotThrowException(actual);
        }

        /// <summary>
        /// Exception thrown when code unexpectedly throws an exception.
        /// </summary>
        public class DoesNotThrowException : Exception
        {
            /// <summary>
            /// Creates a new instance of the <see cref="T:Xunit.Sdk.DoesNotThrowException" /> class.
            /// </summary>
            /// <param name="actual">Actual exception</param>
            public DoesNotThrowException(Exception actual)
                : base(
                    $"Expected no exception, but failed with exception. {actual.GetType().FullName}: {actual.Message}")
            {
            }
        }
    }
}