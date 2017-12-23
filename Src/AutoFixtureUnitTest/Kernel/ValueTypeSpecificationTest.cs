using System;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class ValueTypeSpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Arrange
            // Act
            var sut = new ValueTypeSpecification();
            // Assert
            Assert.IsAssignableFrom<IRequestSpecification>(sut);
        }

        [Fact]
        public void IsSatisfiedByNullThrows()
        {
            // Arrange
            var sut = new ValueTypeSpecification();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => sut.IsSatisfiedBy(null));
        }

        [Theory]
        [InlineData("Ploeh", false)]
        [InlineData(1, false)]
        [InlineData(typeof(object), false)]
        [InlineData(typeof(string), false)]
        [InlineData(typeof(AbstractType), false)]
        [InlineData(typeof(IInterface), false)]
        [InlineData(typeof(MutableValueType), true)]
        [InlineData(typeof(char), false)]
        [InlineData(typeof(ActivityScope), false)]
        [InlineData(typeof(decimal), true)]
        [InlineData(typeof(Nullable<int>), true)]
        public void IsSatisfiedByReturnsCorrectResult(object request, bool expectedResult)
        {
            // Arrange
            var sut = new ValueTypeSpecification();
            // Act
            var result = sut.IsSatisfiedBy(request);
            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}