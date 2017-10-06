using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using System.Collections.Generic;

namespace Ploeh.AutoFixture.NUnit3
{
    /// <summary>
    /// Utility used to create a <see cref="TestMethod"/> instance.
    /// </summary>
    public interface ITestMethodBuilder
    {
        /// <summary>
        /// Builds a <see cref="TestCaseParameters"/> from a method and the argument values.
        /// </summary>
        /// <param name="method">The <see cref="IMethodInfo"/> for which tests are to be constructed.</param>
        /// <param name="suite">The suite to which the tests will be added.</param>
        /// <param name="parameterValues">The argument values generated for the test case.</param>
        /// <param name="autoDataStartIndex">Index at which the automatically generated values start.</param>
        TestMethod Build(IMethodInfo method, Test suite, IEnumerable<object> parameterValues, int autoDataStartIndex);
    }
}
