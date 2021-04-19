using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Interfaces;

namespace AutoFixture.NUnit3
{
    internal class NullSource : ITestCaseSource, IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            return Enumerable.Empty<object>().GetEnumerator();
        }

        public IEnumerable<IReadOnlyList<object>> GetTestCases(IMethodInfo methodInfo)
        {
            return Enumerable.Empty<object[]>();
        }
    }
}