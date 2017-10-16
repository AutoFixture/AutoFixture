using System.Collections.Generic;
using System.Reflection;
using AutoFixture.NUnit2.Addins;
using NUnit.Framework;

namespace AutoFixture.NUnit2.UnitTest
{
    public class FakeDataAttribute : DataAttribute
    {
        private readonly MethodInfo expectedMethod;
        private readonly IEnumerable<object[]> output;

        public FakeDataAttribute(MethodInfo expectedMethod, IEnumerable<object[]> output)
        {
            this.expectedMethod = expectedMethod;
            this.output = output;
        }

        public override IEnumerable<object[]> GetData(MethodInfo method)
        {
            Assert.AreEqual(this.expectedMethod, method);
            
            return this.output;
        }
    }
}
