using System.Linq;
using System.Reflection;
using NUnit.Core;
using NUnit.Core.Extensibility;

namespace Ploeh.AutoFixture.NUnit.Addin.Decorators
{
    public class CustomDecorator : ITestDecorator
    {
        public Test Decorate(Test test, MemberInfo member)
        {
            var hasIgnoreAttribute = Reflect.GetAttributes(((NUnitTestMethod)test).Method, NUnitFramework.IgnoreAttribute, true).Any();

            if (test.GetType() == typeof(NUnitTestMethod) && hasIgnoreAttribute == false)
            {
                return new TestMethodWrapper((NUnitTestMethod) test);
            }

            return test;
        }
    }
}