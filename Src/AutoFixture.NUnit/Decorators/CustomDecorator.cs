using System.Reflection;
using NUnit.Core;
using NUnit.Core.Extensibility;
using NUnit.Framework;

namespace Ploeh.AutoFixture.NUnit.Decorators
{
    public class CustomDecorator : ITestDecorator
    {
        public Test Decorate(Test test, MemberInfo member)
        {
            if (test.GetType() == typeof(NUnitTestMethod) && ((NUnitTestMethod)test).Method.GetCustomAttributes(typeof(IgnoreAttribute), true).Length == 0)
            {
                return new TestMethodWrapper((NUnitTestMethod) test);
            }

            return test;
        }

    }
}