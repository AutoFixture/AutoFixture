using System;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class NoConstructorsSpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Arrange
            // Act
            var sut = new NoConstructorsSpecification();
            // Assert
            Assert.IsAssignableFrom<IRequestSpecification>(sut);
        }

        [Fact]
        public void IsSatisfiedByNullThrows()
        {
            // Arrange
            var sut = new NoConstructorsSpecification();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => sut.IsSatisfiedBy(null));
        }

        [Theory]
        [InlineData("Ploeh", false)]
        [InlineData(1, false)]
        [InlineData(typeof(object), false)]
        [InlineData(typeof(string), false)]
        [InlineData(typeof(AbstractType), true)]
        [InlineData(typeof(IInterface), true)]
        [InlineData(typeof(MutableValueType), false)]
        [InlineData(typeof(char), true)]
        [InlineData(typeof(ActivityScope), true)]
        public void IsSatisfiedByReturnsCorrectResult(object request, bool expectedResult)
        {
            // Arrange
            var sut = new NoConstructorsSpecification();
            // Act
            var result = sut.IsSatisfiedBy(request);
            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}