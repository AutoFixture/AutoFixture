using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Kernel;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class FiniteSequenceRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new FiniteSequenceRelay();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullContainerThrows()
        {
            // Fixture setup
            var sut = new FiniteSequenceRelay();
            var dummyRequest = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(dummyRequest, null));
            // Teardown
        }

        [Fact]
        public void CreateWithAnonymousRequestReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new FiniteSequenceRelay();
            var request = new object();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen(request);
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Theory]
        [InlineData(null)]
        [InlineData(1)]
        [InlineData("foo")]
        public void CreateWithInvalidRequestReturnsCorrectResult(object request)
        {
            // Fixture setup
            var sut = new FiniteSequenceRelay();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen(request);
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWithFiniteSequenceRequestReturnsCorrectResult()
        {
            // Fixture setup
            var request = new object();
            var count = 3;
            var manyRequest = new FiniteSequenceRequest(request, count);

            var expectedResult = new object();
            var container = new DelegatingSpecimenContext { OnResolve = r => request.Equals(r) ? expectedResult : new NoSpecimen() };

            var sut = new FiniteSequenceRelay();
            // Exercise system
            var result = sut.Create(manyRequest, container);
            // Verify outcome
            var actual = Assert.IsAssignableFrom<IEnumerable<object>>(result);
            Assert.True(actual.All(expectedResult.Equals));
            // Teardown
        }
    }
}
