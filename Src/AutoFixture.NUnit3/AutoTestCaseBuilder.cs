using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;

namespace AutoFixture.NUnit3
{
    internal class AutoTestCaseBuilder
    {
        private readonly Func<IFixture> fixtureFactory;
        private readonly NUnitTestCaseBuilder builder = new NUnitTestCaseBuilder();

        public AutoTestCaseBuilder(Func<IFixture> fixtureFactory)
        {
            this.fixtureFactory = fixtureFactory;
        }

        public string Category { get; set; }

        public TestMethod BuildTestMethod(IMethodInfo method, Test test, IReadOnlyList<object> arguments)
        {
            var parameters = method.GetParameters();
            TestCaseParameters args = arguments.Count < parameters.Length
                ? new AutoTestCaseParameters(this.fixtureFactory, parameters, arguments)
                : new TestCaseParameters(arguments.ToArray());

            if (!string.IsNullOrWhiteSpace(this.Category))
            {
                args.Properties.Add("Category", this.Category);
            }

            return this.builder.BuildTestMethod(method, test, args);
        }
    }
}
