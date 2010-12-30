using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Kernel;
using System.Reflection;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class MethodContextTest
    {
        [Fact]
        public void InitializeWithNullFixtureThrows()
        {
            // Fixture setup
            var dummyMethod = typeof(object).GetMethods().First();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new MethodContext(null, dummyMethod));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullMethodThrows()
        {
            // Fixture setup
            var dummyFixture = new Fixture();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new MethodContext(dummyFixture, null));
            // Teardown
        }

        [Fact]
        public void FixtureIsCorrect()
        {
            // Fixture setup
            var expectedFixture = new Fixture();
            var dummyMethod = typeof(object).GetMethods().First();
            var sut = new MethodContext(expectedFixture, dummyMethod);
            // Exercise system
            IFixture result = sut.Fixture;
            // Verify outcome
            Assert.Equal(expectedFixture, result);
            // Teardown
        }

        [Fact]
        public void MethodIsCorrect()
        {
            // Fixture setup
            var dummyFixture = new Fixture();
            var expectedMethod = typeof(object).GetMethods().First();
            var sut = new MethodContext(dummyFixture, expectedMethod);
            // Exercise system
            MethodInfo result = sut.Method;
            // Verify outcome
            Assert.Equal(expectedMethod, result);
            // Teardown
        }
    }
}
