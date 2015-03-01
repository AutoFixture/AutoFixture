using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.Xunit
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class InfiniteDataAttribute : DataAttribute
    {
        private readonly DataAttribute _decorated;

        public InfiniteDataAttribute(DataAttribute decorated)
        {
            _decorated = decorated;
        }

        public override IEnumerable<object[]> GetData(MethodInfo methodUnderTest, Type[] parameterTypes)
        {
            while (true)
            {
                foreach (var dataSet in _decorated.GetData(methodUnderTest, parameterTypes))
                    yield return dataSet;
            }
        }
    }
}
