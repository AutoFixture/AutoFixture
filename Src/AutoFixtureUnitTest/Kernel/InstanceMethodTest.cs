using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;
using Xunit.Extensions;

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

        [Fact]
        public void ConstructWithNullMethodThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new InstanceMethod(null));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(DateTime))]
        public void MethodIsCorrect(Type type)
        {
            // Fixture setup
            var expectedMethod = type.GetMethods(BindingFlags.Public | BindingFlags.Instance).First();
            var sut = new InstanceMethod(expectedMethod);
            // Exercise system
            MethodInfo result = sut.Method;
            // Verify outcome
            Assert.Equal(expectedMethod, result);
            // Teardown
        }
    }
}
