using System;

namespace Ploeh.AutoFixture.NUnit.UnitTest
{
    public class SubclassAutoTestCaseAttribute : AutoTestCaseAttribute
    {
        public SubclassAutoTestCaseAttribute(Type testClassType, string methodName, IFixture fixture)
            : base(testClassType, methodName, fixture, new object[0])
        {
        }
    }
}
