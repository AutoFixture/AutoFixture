//http://youtrack.jetbrains.com/issue/RSRP-205480

using System.Reflection;
using NUnit.Core;
using NUnit.Core.Extensibility;

namespace Ploeh.AutoFixture.NUnit2.Reharper
{
    /// <summary>
    /// TestDecorator is used to add functionality to
    /// another Test, which it aggregates.
    /// </summary>
    internal class TestDecorator : ITestDecorator
    {
        public Test Decorate(Test test, MemberInfo memberInfo)
        {
            if (test.GetType() == typeof(NUnitTestMethod))
            {
                if(Reflect.GetAttributes(((NUnitTestMethod)test).Method, NUnitFramework.IgnoreAttribute, true).Length == 0)
                    return new TestMethodWrapper((NUnitTestMethod) test);
            }

            return test;
        }
    }
}