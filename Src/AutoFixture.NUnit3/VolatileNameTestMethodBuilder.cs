using System;
using System.Collections.Generic;
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
        public TestMethod Build(IMethodInfo method, Test suite, IEnumerable<object> parameterValues, int autoDataStartIndex)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            if (parameterValues == null)
            {
                throw new ArgumentNullException(nameof(parameterValues));
            }

            return new NUnitTestCaseBuilder().BuildTestMethod(method, suite, GetParametersForMethod(parameterValues));
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", 
            Justification = "This method is always expected to return an instance of the TestCaseParameters class.")]
        private static TestCaseParameters GetParametersForMethod(IEnumerable<object> parameterValues)
        {
            try
            {
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
