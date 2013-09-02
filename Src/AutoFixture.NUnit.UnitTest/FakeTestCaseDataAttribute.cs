using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Ploeh.AutoFixture.NUnit.UnitTest
{
    public class FakeTestCaseDataAttribute : TestCaseDataAttribute
    {
        private readonly MethodInfo expectedMethod;
        private readonly IEnumerable<object[]> output;

        public FakeTestCaseDataAttribute(MethodInfo expectedMethod, IEnumerable<object[]> output)
        {
            this.expectedMethod = expectedMethod;
            this.output = output;
        }

        public override IEnumerable<object[]> GetArguments(MethodInfo method)
        {
            Assert.Equal(this.expectedMethod, method);
            
            return this.output;
        }
    }
}
