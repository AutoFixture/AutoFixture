using System;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class SeedRequestSpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Arrange
            var dummyType = typeof(object);
            // Act
            var sut = new SeedRequestSpecification(dummyType);
            // Assert
            Assert.IsAssignableFrom<IRequestSpecification>(sut);
        }

        [Fact]
        public void InitializeWithNullTypeThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new SeedRequestSpecification(null));
        }

        [Fact]
        public void TargetTypeIsCorrect()
        {
            // Arrange
            var expectedType = typeof(DayOfWeek);
            var sut = new SeedRequestSpecification(expectedType);
            // Act
            Type result = sut.TargetType;
            // Assert
            Assert.Equal(expectedType, result);
        }

        [Fact]
        public void IsSatisfiedByNullThrows()
        {
            // Arrange
            var dummyType = typeof(object);
            var sut = new SeedRequestSpecification(dummyType);
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.IsSatisfiedBy(null));
        }

        [Fact]
        public void IsSatisfiedByNonSeedReturnsCorrectResult()
        {
            // Arrange
            var dummyType = typeof(object);
            var sut = new SeedRequestSpecification(dummyType);
            var nonSeedRequest = new object();
            // Act
            var result = sut.IsSatisfiedBy(nonSeedRequest);
            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(typeof(object), typeof(object), true)]
        [InlineData(typeof(string), typeof(string), true)]
        [InlineData(typeof(string), typeof(int), false)]
        [InlineData(typeof(PropertyHolder<string>), typeof(FieldHolder<string>), false)]
        public void IsSatisfiedByReturnsCorrectResult(Type specType, Type seedRequestType, bool expectedResult)
        {
            // Arrange
            var sut = new SeedRequestSpecification(specType);
            var dummySeed = new object();
            var seededRequest = new SeededRequest(seedRequestType, dummySeed);
            // Act
            var result = sut.IsSatisfiedBy(seededRequest);
            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
