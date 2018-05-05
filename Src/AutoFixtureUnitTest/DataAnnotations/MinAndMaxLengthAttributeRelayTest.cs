using System;
using System.ComponentModel.DataAnnotations;
using AutoFixture.DataAnnotations;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.DataAnnotations
{
    public class MinAndMaxLengthAttributeRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Act
            var sut = new MinAndMaxLengthAttributeRelay();

            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestReturnsCorrectResult()
        {
            // Arrange
            var sut = new MinAndMaxLengthAttributeRelay();

            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(null, dummyContext);

            // Assert
            Assert.Equal(new NoSpecimen(), result);
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            // Arrange
            var sut = new MinAndMaxLengthAttributeRelay();
            var dummyRequest = new object();

            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(dummyRequest, null));
        }

        [Fact]
        public void CreateWithAnonymousRequestReturnsCorrectResult()
        {
            // Arrange
            var sut = new MinAndMaxLengthAttributeRelay();
            var dummyRequest = new object();

            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(dummyRequest, dummyContainer);

            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(1)]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        [InlineData(typeof(Version))]
        public void CreateWithNonConstrainedStringRequestReturnsCorrectResult(object request)
        {
            // Arrange
            var sut = new MinAndMaxLengthAttributeRelay();

            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContext);

            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void CreateWithStringRequestConstrainedbyMinLengthReturnsCorrectResult(int min)
        {
            // Arrange
            var minLengthAttribute = new MinLengthAttribute(min);

            var request = new FakeMemberInfo(
                new ProvidedAttribute(minLengthAttribute, true));

            var expectedRequest = new ConstrainedStringRequest(min, Int32.MaxValue);
            var expectedResult = new object();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedRequest.Equals(r) ? expectedResult : new NoSpecimen()
            };
            var sut = new MinAndMaxLengthAttributeRelay();

            // Act
            var result = sut.Create(request, context);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void CreateWithStringRequestConstrainedbyMaxLengthReturnsCorrectResult(int max)
        {
            // Arrange
            var maxLengthAttribute = new MaxLengthAttribute(max);

            var request = new FakeMemberInfo(
                new ProvidedAttribute(maxLengthAttribute, true));

            var expectedRequest = new ConstrainedStringRequest(0, max);
            var expectedResult = new object();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedRequest.Equals(r) ? expectedResult : new NoSpecimen()
            };
            var sut = new MinAndMaxLengthAttributeRelay();

            // Act
            var result = sut.Create(request, context);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(1, 4)]
        [InlineData(2, 5)]
        [InlineData(3, 6)]
        public void CreateWithStringRequestConstrainedbyMinAndMaxLengthReturnsCorrectResult(int min, int max)
        {
            // Arrange
            var minLengthAttribute = new MinLengthAttribute(min);
            var maxLengthAttribute = new MaxLengthAttribute(max);

            var request = new FakeMemberInfo(
                new ProvidedAttribute(minLengthAttribute, true),
                new ProvidedAttribute(maxLengthAttribute, true));

            var expectedRequest = new ConstrainedStringRequest(min, max);
            var expectedResult = new object();

            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedRequest.Equals(r) ? expectedResult : new NoSpecimen()
            };
            var sut = new MinAndMaxLengthAttributeRelay();

            // Act
            var result = sut.Create(request, context);

            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}