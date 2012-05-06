using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class OmitRecursionGuardTest
    {
        [Fact]
        public void SutIsRecursionGuard()
        {
            // Fixture setup
            // Exercise system
            var dummy = new DelegatingSpecimenBuilder();
            var sut = new OmitRecursionGuard(dummy);
            // Verify outcome
            Assert.IsAssignableFrom<RecursionGuard>(sut);
            // Teardown
        }

        [Fact]
        public void HandleRecursiveRequestReturnsCorrectResult()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new OmitRecursionGuard(dummyBuilder);
            // Exercise system
            var dummyRequest = new object();
            var actual = sut.HandleRecursiveRequest(dummyRequest);
            // Verify outcome
            var expected = new OmitSpecimen();
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void ConstructorThrowsOnNullComparer()
        {
            // Fixture setup
            var dummy = new DelegatingSpecimenBuilder();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new OmitRecursionGuard(dummy, null));
            // Teardown
        }
    }
}
