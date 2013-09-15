using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace Ploeh.AutoFixture.NUnit2.UnitTest
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
            Assert.AreEqual(this.expectedMethod, method);
            
            return this.output;
        }
    }
}
