using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class StableFiniteSequenceRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new StableFiniteSequenceRelay();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            // Arrange
            var sut = new StableFiniteSequenceRelay();
            var dummyRequest = new object();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(dummyRequest, null));
        }

        [Fact]
        public void CreateWithAnonymousRequestReturnsCorrectResult()
        {
            // Arrange
            var sut = new StableFiniteSequenceRelay();
            var request = new object();
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContext);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(1)]
        [InlineData("foo")]
        public void CreateWithInvalidRequestReturnsCorrectResult(object request)
        {
            // Arrange
            var sut = new StableFiniteSequenceRelay();
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContext);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWithFiniteSequenceRequestReturnsCorrectResult()
        {
            // Arrange
            var request = new object();
            var count = 3;
            var manyRequest = new FiniteSequenceRequest(request, count);

            var expectedResult = new object();
            var context = new DelegatingSpecimenContext { OnResolve = r => request.Equals(r) ? expectedResult : new NoSpecimen() };

            var sut = new StableFiniteSequenceRelay();
            // Act
            var result = sut.Create(manyRequest, context);
            // Assert
            var actual = Assert.IsAssignableFrom<IEnumerable<object>>(result);
            Assert.True(actual.All(expectedResult.Equals));
        }

        [Fact]
        public void CreateReturnsStableSequence()
        {
            // Arrange
            var request = new object();
            var count = 3;
            var manyRequest = new FiniteSequenceRequest(request, count);

            var context = new DelegatingSpecimenContext { OnResolve = r => request.Equals(r) ? new object() : new NoSpecimen() };

            var sut = new StableFiniteSequenceRelay();
            // Act
            var result = sut.Create(manyRequest, context);
            // Assert
            var expected = Assert.IsAssignableFrom<IEnumerable<object>>(result);
            Assert.True(expected.SequenceEqual(expected));
        }

        [Fact]
        public void CreateFiltersOmitSpecimenInstances()
        {
            // Arrange
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
            // Act
            var actual = sut.Create(manyRequest, context);
            // Assert
            var iter = Assert.IsAssignableFrom<IEnumerable<object>>(actual);
            Assert.True(
                results.Where(x => !(x is OmitSpecimen)).SequenceEqual(iter),
                "Actual sequence is not equal to expected sequence.");
        }
    }
}
