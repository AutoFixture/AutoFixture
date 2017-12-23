using System;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class AnyTypeSpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Arrange
            // Act
            var sut = new AnyTypeSpecification();
            // Assert
            Assert.IsAssignableFrom<IRequestSpecification>(sut);
        }

        [Fact]
        public void IsSatisfiedByNullThrows()
        {
            // Arrange
            var sut = new AnyTypeSpecification();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => sut.IsSatisfiedBy(null));
        }

        [Theory]
        [InlineData("Ploeh", false)]
        [InlineData(1, false)]
        [InlineData(typeof(object), true)]
        [InlineData(typeof(string), true)]
        [InlineData(typeof(PropertyHolder<DateTimeOffset>), true)]
        public void IsSatisfiedByReturnsCorrectResult(object request, bool expectedResult)
        {
            // Arrange
            var sut = new AnyTypeSpecification();
            // Act
            var result = sut.IsSatisfiedBy(request);
            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
