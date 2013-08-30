using System.Linq;
using System.Reflection;
using NUnit.Core;
using NUnit.Core.Extensibility;

namespace Ploeh.AutoFixture.NUnit.Addins.Decorators
{
    public class CustomDecorator : ITestDecorator
    {
        public Test Decorate(Test test, MemberInfo member)
        {
            if (test.GetType() == typeof(NUnitTestMethod))
            {
                if(Reflect.GetAttributes(((NUnitTestMethod)test).Method, NUnitFramework.IgnoreAttribute, true).Any())
                    return new TestMethodWrapper((NUnitTestMethod) test);
            }

            return test;
        }
    }
}