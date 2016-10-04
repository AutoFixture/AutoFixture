using System;
using System.Linq;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework.Internal.Builders;

namespace Ploeh.AutoFixture.NUnit3
{
    /// Creates <see cref="TestMethod"/> instances with name that includes actual argument values.
    /// Name is volatile and varies on the argument values.
    public class VolatileNameTestMethodBuilder : ITestMethodBuilder
    {
        /// <inheritdoc />
        public TestMethod Build(IMethodInfo method, Test suite, Func<object[]> argsFactory, int autoDataStartIndex)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            if (argsFactory == null)
            {
                throw new ArgumentNullException(nameof(argsFactory));
            }

            return new NUnitTestCaseBuilder().BuildTestMethod(method, suite, GetParametersForMethod(argsFactory));
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", 
            Justification = "This method is always expected to return an instance of the TestCaseParameters class.")]
        private static TestCaseParameters GetParametersForMethod(Func<object[]> argsFactory)
        {
            try
            {
                var parameterValues = argsFactory.Invoke();
                return GetParametersForMethod(parameterValues.ToArray());
            }
            catch (Exception ex)
            {
                return new TestCaseParameters(ex);
            }
        }

        private static TestCaseParameters GetParametersForMethod(object[] args)
        {
            return new TestCaseParameters(args);
        }
    }
}
