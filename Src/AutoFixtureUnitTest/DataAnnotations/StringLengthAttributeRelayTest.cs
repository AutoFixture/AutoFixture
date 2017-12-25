using System;
using System.ComponentModel.DataAnnotations;
using AutoFixture.DataAnnotations;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.DataAnnotations
{
    public class StringLengthAttributeRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new StringLengthAttributeRelay();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestReturnsCorrectResult()
        {
            // Arrange
            var sut = new StringLengthAttributeRelay();
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
            var sut = new StringLengthAttributeRelay();
            var dummyRequest = new object();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(dummyRequest, null));
        }

        [Fact]
        public void CreateWithAnonymousRequestReturnsCorrectResult()
        {
            // Arrange
            var sut = new StringLengthAttributeRelay();
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
            var sut = new StringLengthAttributeRelay();
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
        public void CreateWithConstrainedStringRequestReturnsCorrectResult(int maximum)
        {
            // Arrange
            var stringLengthAttribute = new StringLengthAttribute(maximum);
            var providedAttribute = new ProvidedAttribute(stringLengthAttribute, true);
            var request = new FakeMemberInfo(providedAttribute);
            var expectedRequest = new ConstrainedStringRequest(stringLengthAttribute.MaximumLength);
            var expectedResult = new object();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedRequest.Equals(r) ? expectedResult : new NoSpecimen()
            };
            var sut = new StringLengthAttributeRelay();
            // Act
            var result = sut.Create(request, context);
            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void CreateWithStringRequestConstrainedbyMinimunLengthReturnsCorrectResult(int maximum)
        {
            // Arrange
            var stringLengthAttribute = new StringLengthAttribute(maximum) { MinimumLength = 1 };
            var providedAttribute = new ProvidedAttribute(stringLengthAttribute, true);
            var request = new FakeMemberInfo(providedAttribute);
            var expectedRequest = new ConstrainedStringRequest(stringLengthAttribute.MinimumLength, stringLengthAttribute.MaximumLength);
            var expectedResult = new object();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedRequest.Equals(r) ? expectedResult : new NoSpecimen()
            };
            var sut = new StringLengthAttributeRelay();
            // Act
            var result = sut.Create(request, context);
            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
