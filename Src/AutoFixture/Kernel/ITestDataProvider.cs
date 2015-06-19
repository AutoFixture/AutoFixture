using System.Collections.Generic;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Provides automatically generated arguments for a test method.
    /// </summary>
    public interface ITestDataProvider
    {
        /// <summary>
        /// Returns arguments for invoking the given <paramref name="testMethod"/>.
        /// </summary>
        /// <param name="testMethod">A <see cref="MethodInfo"/> instance representing a test method.</param>
        IEnumerable<object> GetData(MethodInfo testMethod);
    }
}
