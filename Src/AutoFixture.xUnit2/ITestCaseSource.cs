using System.Collections.Generic;
using System.Reflection;

namespace AutoFixture.Xunit2
{
    public interface ITestCaseSource
    {
        IEnumerable<IEnumerable<object>> GetTestCases(MethodInfo method);
    }
}
