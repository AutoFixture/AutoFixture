using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class InstanceMethodTest
    {
        [Fact]
        public void SutIsMethod()
        {
            // Fixture setup
            var dummyMethod = typeof(object).GetMethods(BindingFlags.Public | BindingFlags.Instance).First();
            // Exercise system
            var sut = new InstanceMethod(dummyMethod);
            // Verify outcome
            Assert.IsAssignableFrom<IMethod>(sut);
            // Teardown
        }
    }
}
