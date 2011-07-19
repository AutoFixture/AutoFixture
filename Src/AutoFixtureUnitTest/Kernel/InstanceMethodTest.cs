using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;
using Xunit.Extensions;
using Ploeh.TestTypeFoundation;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class InstanceMethodTest
    {
        [Fact]
        public void SutIsMethod()
        {
            // Fixture setup
            var dummyMethod = typeof(object).GetMethods(BindingFlags.Public | BindingFlags.Instance).First();
            var dummyOwner = new object();
            // Exercise system
            var sut = new InstanceMethod(dummyMethod, dummyOwner);
            // Verify outcome
            Assert.IsAssignableFrom<IMethod>(sut);
            // Teardown
        }

        [Fact]
        public void ConstructWithNullMethodThrows()
        {
            // Fixture setup
            var dummyOwner = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new InstanceMethod(null, dummyOwner));
            // Teardown
        }

        [Fact]
        public void ConstructWithNullOwnerThrows()
        {
            // Fixture setup
            var dummyMethod = typeof(object).GetMethods(BindingFlags.Public | BindingFlags.Instance).First();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new InstanceMethod(dummyMethod, null));
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
            var dummyOwner = new object();
            var sut = new InstanceMethod(expectedMethod, dummyOwner);
            // Exercise system
            MethodInfo result = sut.Method;
            // Verify outcome
            Assert.Equal(expectedMethod, result);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(GuardedMethodHost), 0)]
        [InlineData(typeof(GuardedMethodHost), 1)]
        [InlineData(typeof(GuardedMethodHost), 2)]
        [InlineData(typeof(GuardedMethodHost), 3)]
        [InlineData(typeof(GuardedMethodHost), 4)]
        [InlineData(typeof(GuardedMethodHost), 5)]
        [InlineData(typeof(GuardedMethodHost), 6)]
        public void ParametersReturnsCorrectResult(Type type, int index)
        {
            // Fixture setup
            var method = type.GetMethods(BindingFlags.Public | BindingFlags.Instance).ElementAt(index);
            var expectedParameters = method.GetParameters();

            var dummyOwner = new object();
            var sut = new InstanceMethod(method, dummyOwner);
            // Exercise system
            var result = sut.Parameters;
            // Verify outcome
            Assert.True(expectedParameters.SequenceEqual(result));
            // Teardown
        }
    }
}
