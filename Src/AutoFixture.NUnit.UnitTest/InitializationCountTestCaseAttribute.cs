using System;

namespace Ploeh.AutoFixture.NUnit.UnitTest
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class InitializationCountTestCaseAttribute : AutoTestCaseAttribute
    {
        public InitializationCountTestCaseAttribute(Type testClassType, string methodName)
            : base(testClassType, methodName, CreateFixture(methodName), new object[0])
        {
        }

        private static IFixture CreateFixture(string freezeString)
        {
            var fixture = new Fixture();
            fixture.Register(() => freezeString);
            return fixture;
        }
    }
}
