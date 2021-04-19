using System.Collections.Generic;
using NUnit.Framework.Interfaces;

namespace AutoFixture.NUnit3
{
    internal interface ITestCaseSource
    {
        IEnumerable<IReadOnlyList<object>> GetTestCases(IMethodInfo method);
    }
}