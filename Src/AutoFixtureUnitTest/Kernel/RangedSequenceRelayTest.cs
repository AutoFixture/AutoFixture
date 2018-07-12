using System;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class RangedSequenceRelayTest
    {
        [Fact]
        public void ShouldThrowIfContextIsNull()
        {
            // Arrange
            var request = new object();
            var sut = new RangedSequenceRelay();

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() =>
                sut.Create(request, context: null));
            Assert.Equal("context", ex.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(typeof(object))]
        [InlineData(typeof(int))]
        public void ShouldReturnNoSpecimenForNonSupportedRequests(object request)
        {
            // Arrange
            var context = new DelegatingSpecimenContext();
            var sut = new RangedSequenceRelay();

            // Act
            var result = sut.Create(request, context);

            // Assert
            Assert.IsType<NoSpecimen>(result);
        }

        [Fact]
        public void ShouldEmitRandomNumberRequestWithCorrectBoundaries()
        {
            // Arrange
            var min = 10;
            var max = 20;
            var rsr = new RangedSequenceRequest(typeof(object), min, max);

            RangedNumberRequest capturedNumberRequest = null;
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r =>
                {
                    if (r is RangedNumberRequest rnr)
                        capturedNumberRequest = rnr;

                    return new NoSpecimen();
                }
            };

            var sut = new RangedSequenceRelay();

            // Act
            sut.Create(rsr, context);

            // Assert
            Assert.NotNull(capturedNumberRequest);
            Assert.Equal(min, capturedNumberRequest.Minimum);
            Assert.Equal(max, capturedNumberRequest.Maximum);
        }

        [Fact]
        public void ShouldReturnNoSpecimenIfUnableToGetRandomLength()
        {
            // Arrange
            var rsr = new RangedSequenceRequest(typeof(object), minLength: 10, maxLength: 20);
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r =>
                {
                    if (r is RangedNumberRequest)
                        return new NoSpecimen();

                    return 42;
                }
            };
            var sut = new RangedSequenceRelay();

            // Act
            var result = sut.Create(rsr, context);

            // Assert
            Assert.IsType<NoSpecimen>(result);
        }

        [Fact]
        public void ShouldUseInitialRequestAndReturnedRandomNumberForFiniteSequenceRequest()
        {
            // Arrange
            var request = new object();
            var rsr = new RangedSequenceRequest(request, minLength: 10, maxLength: 20);
            var sequenceLength = 42;
            FiniteSequenceRequest capturedSequenceRequest = null;
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r =>
                {
                    if (r is RangedNumberRequest)
                        return sequenceLength;

                    if (r is FiniteSequenceRequest fsr)
                        capturedSequenceRequest = fsr;

                    return new NoSpecimen();
                }
            };

            var sut = new RangedSequenceRelay();

            // Act
            sut.Create(rsr, context);

            // Assert
            var expectedSequenceRequest = new FiniteSequenceRequest(request, sequenceLength);
            Assert.Equal(expectedSequenceRequest, capturedSequenceRequest);
        }

        [Fact]
        public void ShouldUseFiniteSequenceRequestResultForResult()
        {
            // Arrange
            var rsr = new RangedSequenceRequest(typeof(object), minLength: 10, maxLength: 20);
            var expectedResult = new object();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r =>
                {
                    if (r is RangedNumberRequest)
                        return 42;

                    if (r is FiniteSequenceRequest)
                        return expectedResult;

                    return new NoSpecimen();
                }
            };
            var sut = new RangedSequenceRelay();

            // Act
            var actualResult = sut.Create(rsr, context);

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }
    }
}