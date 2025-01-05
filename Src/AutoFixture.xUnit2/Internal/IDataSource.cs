using System.Collections.Generic;
using System.Reflection;

namespace AutoFixture.Xunit2.Internal
{
    /// <summary>
    /// Exposes the factory method for a sequence of test data.
    /// </summary>
    public interface IDataSource
    {
        /// <summary>
        /// Returns the test data provided by the source.
        /// </summary>
        /// <param name="method">The target method for which to provide the arguments.</param>
        /// <returns>Returns a sequence of argument collections.</returns>
        IEnumerable<object[]> GetData(MethodInfo method);
    }
}