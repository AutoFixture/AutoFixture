using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoFixture.NUnit3
{
    internal class StaticMethodSource : TestCaseSourceBase
    {
        public StaticMethodSource(MethodInfo method, IEnumerable<object> parameters)
        {
            this.Method = method;
            this.Parameters = parameters;
        }

        public MethodInfo Method { get; }

        public IEnumerable<object> Parameters { get; }

        public override IEnumerator GetEnumerator()
        {
            return GetMethodData(this.Method, this.Parameters).GetEnumerator();
        }

        private static IEnumerable GetMethodData(MethodInfo method, IEnumerable<object> parameters)
        {
            if (method.Invoke(null, parameters.ToArray()) is IEnumerable enumerable)
            {
                return enumerable;
            }
            else
            {
                return Enumerable.Empty<object>();
            }
        }
    }
}