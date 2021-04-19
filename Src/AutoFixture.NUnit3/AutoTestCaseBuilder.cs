using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;

namespace AutoFixture.NUnit3
{
    internal class AutoTestCaseBuilder
    {
        private readonly NUnitTestCaseBuilder builder = new NUnitTestCaseBuilder();

        public TestMethod BuildTestMethod(IMethodInfo method, Test test, AutoTestCaseParameters parameters)
        {
            return this.builder.BuildTestMethod(method, test, parameters.GetParameters(method));
        }
    }
}
