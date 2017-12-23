using System;
using System.ComponentModel.DataAnnotations;
using AutoFixture.DataAnnotations;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.DataAnnotations
{
    public class RegularExpressionAttributeRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new RegularExpressionAttributeRelay();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestReturnsCorrectResult()
        {
            // Arrange
            var sut = new RegularExpressionAttributeRelay();
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
            var sut = new RegularExpressionAttributeRelay();
            var dummyRequest = new object();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(dummyRequest, null));
        }

        [Fact]
        public void CreateWithAnonymousRequestReturnsCorrectResult()
        {
            // Arrange
            var sut = new RegularExpressionAttributeRelay();
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
        public void CreateWithNonRegularExpressionAttributeRequestReturnsCorrectResult(object request)
        {
            // Arrange
            var sut = new RegularExpressionAttributeRelay();
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContext);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("[0-9]")]
        [InlineData("[A-Z]")]
        [InlineData("[a-z]")]
        public void CreateWithRegularExpressionAttributeRequestReturnsCorrectResult(string pattern)
        {
            // Arrange
            var regularExpressionAttribute = new RegularExpressionAttribute(pattern);
            var providedAttribute = new ProvidedAttribute(regularExpressionAttribute, true);
            var request = new FakeMemberInfo(providedAttribute);
            var expectedRequest = new RegularExpressionRequest(regularExpressionAttribute.Pattern);
            var expectedResult = new object();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedRequest.Equals(r) ? expectedResult : new NoSpecimen()
            };
            var sut = new RegularExpressionAttributeRelay();
            // Act
            var result = sut.Create(request, context);
            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
