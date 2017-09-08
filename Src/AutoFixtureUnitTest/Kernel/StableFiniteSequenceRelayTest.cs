using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Kernel;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class StableFiniteSequenceRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new StableFiniteSequenceRelay();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            // Fixture setup
            var sut = new StableFiniteSequenceRelay();
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
            var sut = new StableFiniteSequenceRelay();
            var request = new object();
            // Exercise system
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContext);
            // Verify outcome
            var expectedResult = new NoSpecimen();
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
            var sut = new StableFiniteSequenceRelay();
            // Exercise system
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContext);
            // Verify outcome
            var expectedResult = new NoSpecimen();
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
            var context = new DelegatingSpecimenContext { OnResolve = r => request.Equals(r) ? expectedResult : new NoSpecimen() };

            var sut = new StableFiniteSequenceRelay();
            // Exercise system
            var result = sut.Create(manyRequest, context);
            // Verify outcome
            var actual = Assert.IsAssignableFrom<IEnumerable<object>>(result);
            Assert.True(actual.All(expectedResult.Equals));
            // Teardown
        }

        [Fact]
        public void CreateReturnsStableSequence()
        {
            // Fixture setup
            var request = new object();
            var count = 3;
            var manyRequest = new FiniteSequenceRequest(request, count);

            var context = new DelegatingSpecimenContext { OnResolve = r => request.Equals(r) ? new object() : new NoSpecimen() };

            var sut = new StableFiniteSequenceRelay();
            // Exercise system
            var result = sut.Create(manyRequest, context);
            // Verify outcome
            var expected = Assert.IsAssignableFrom<IEnumerable<object>>(result);
            Assert.True(expected.SequenceEqual(expected));
            // Teardown
        }

        [Fact]
        public void CreateFiltersOmitSpecimenInstances()
        {
            // Fixture setup
            var request = new object();
            var count = 3;
            var manyRequest = new FiniteSequenceRequest(request, count);

            var results = new object[]
            {
                new object(),
                new OmitSpecimen(),
                new object()
            };
            var q = new Queue<object>(results);
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => request.Equals(r) ? q.Dequeue() : new NoSpecimen()
            };

            var sut = new StableFiniteSequenceRelay();
            // Exercise system
            var actual = sut.Create(manyRequest, context);
            // Verify outcome
            var iter = Assert.IsAssignableFrom<IEnumerable<object>>(actual);
            Assert.True(
                results.Where(x => !(x is OmitSpecimen)).SequenceEqual(iter),
                "Actual sequence is not equal to expected sequence.");
            // Teardown
        }
    }
}
