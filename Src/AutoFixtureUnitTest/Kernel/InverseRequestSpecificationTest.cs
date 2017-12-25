using System;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class InverseRequestSpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Arrange
            var dummySpec = new DelegatingRequestSpecification();
            // Act
            var sut = new InverseRequestSpecification(dummySpec);
            // Assert
            Assert.IsAssignableFrom<IRequestSpecification>(sut);
        }

        [Fact]
        public void InitializeWithNullSpecificationThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => new InverseRequestSpecification(null));
        }

        [Fact]
        public void SpecificationIsCorrectly()
        {
            // Arrange
            var expectedSpec = new DelegatingRequestSpecification();
            var sut = new InverseRequestSpecification(expectedSpec);
            // Act
            IRequestSpecification result = sut.Specification;
            // Assert
            Assert.Equal(expectedSpec, result);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void IsSatisfiedByReturnsCorrectResult(bool decoratedResult)
        {
            // Arrange
            var spec = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => decoratedResult };
            var sut = new InverseRequestSpecification(spec);
            // Act
            var dummyRequest = new object();
            var result = sut.IsSatisfiedBy(dummyRequest);
            // Assert
            Assert.Equal(!decoratedResult, result);
        }

        [Fact]
        public void IsSatisfiedByInvokesDecoratedSpecWithCorrectRequest()
        {
            // Arrange
            var expectedRequest = new object();
            var verified = false;
            var mock = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => verified = expectedRequest == r };
            var sut = new InverseRequestSpecification(mock);
            // Act
            sut.IsSatisfiedBy(expectedRequest);
            // Assert
            Assert.True(verified, "Mock verified");
        }
    }
}
