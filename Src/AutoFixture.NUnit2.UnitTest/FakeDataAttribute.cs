using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Ploeh.AutoFixture.NUnit2.Addins;

namespace Ploeh.AutoFixture.NUnit2.UnitTest
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
