using System;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class NullableEnumRequestSpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Arrange
            // Act
            var sut = new NullableEnumRequestSpecification();
            // Assert
            Assert.IsAssignableFrom<IRequestSpecification>(sut);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("foo")]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(true)]
        [InlineData(false)]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        [InlineData(typeof(Version))]
        [InlineData(typeof(TriState))]
        [InlineData(typeof(int?))]
        [InlineData(typeof(PropertyHolder<int>))]
        [InlineData(typeof(DoubleFieldHolder<int, string>))]
        public void IsSatisfiedReturnsFalseOnRequestWhichIsNotRequestForNullableEnum(object request)
        {
            // Arrange
            var sut = new NullableEnumRequestSpecification();
            // Act
            var result = sut.IsSatisfiedBy(request);
            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(typeof(TriState?))]
        [InlineData(typeof(ConsoleColor?))]
        public void IsSatisfiedReturnsTrueForRequestForNullableEnum(object request)
        {
            // Arrange
            var sut = new NullableEnumRequestSpecification();
            // Act
            var result = sut.IsSatisfiedBy(request);
            // Assert
            Assert.True(result);
        }
    }
}
