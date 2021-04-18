using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework.Interfaces;

namespace AutoFixture.NUnit3
{
    internal abstract class TestCaseSourceBase : ITestCaseSource, IEnumerable
    {
        public abstract IEnumerator GetEnumerator();

        public IEnumerable<IReadOnlyList<object>> GetTestCases(IMethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();
            if (parameters.Length == 0)
            {
                yield break;
            }

            var enumerator = this.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var value = enumerator.Current;

                if (parameters[0].ParameterType.GetTypeInfo().IsInstanceOfType(value))
                {
                    yield return new[] { value };
                }
                else if (value is IEnumerable values)
                {
                    yield return values.OfType<object>().ToList();
                }
            }
        }
    }
}