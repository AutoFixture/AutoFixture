using System;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class ElementsBuilderTest
    {
        [Fact]
        public void CreateWithNullShouldThrow()
        {
            // Arrange
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ElementsBuilder<int>(null));
        }

        [Fact]
        public void CreateWithEmptyCollectionShouldThrow()
        {
            // Arrange
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new ElementsBuilder<int>(Enumerable.Empty<int>()));
        }

        [Fact]
        public void CreateWithZeroArgumentsShouldThrow()
        {
            // Arrange
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new ElementsBuilder<int>());
        }

        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new ElementsBuilder<int>(42);
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNonCorrectTypeRequestWillReturnNoSpecimen()
        {
            // Arrange
            var dummyContext = new DelegatingSpecimenContext();
            var sut = new ElementsBuilder<int>(42);
            // Act
            var result = sut.Create(typeof(string), dummyContext);
            // Assert
            Assert.True(result is NoSpecimen);
        }

        [Fact]
        public void CreateWithCorrectTypeRequestWillReturnCorrectTypeSpecimen()
        {
            // Arrange
            var dummyContext = new DelegatingSpecimenContext();
            var sut = new ElementsBuilder<int>(42);
            // Act
            var result = sut.Create(typeof(int), dummyContext);
            // Assert
            Assert.Equal(typeof(int), result.GetType());
        }

        [Fact]
        public void CreateWithOneElementCollectionWillReturnThatElement()
        {
            // Arrange
            var dummyContext = new DelegatingSpecimenContext();
            var sut = new ElementsBuilder<int>(42);
            // Act
            var result = sut.Create(typeof(int), dummyContext);
            // Assert
            Assert.Equal(42, result);
        }

        [Fact]
        public void CreateReturnsElementFromTheCollection()
        {
            // Arrange
            var collection = new[] { "foo", "bar", "qux" };
            var dummyContext = new DelegatingSpecimenContext();
            var sut = new ElementsBuilder<string>(collection);
            // Act
            var result = sut.Create(typeof(string), dummyContext);
            // Assert
            Assert.Contains(result, collection);
        }

        [Fact]
        public void CreateDoesNotReturnTheSameElementTwiceWhenCalledTwoTimesWithTwoElementsInCollection()
        {
            // Arrange
            var dummyContext = new DelegatingSpecimenContext();
            var sut = new ElementsBuilder<int>(42, 7);
            // Act
            var result1 = sut.Create(typeof(int), dummyContext);
            var result2 = sut.Create(typeof(int), dummyContext);
            // Assert
            Assert.NotEqual(result1, result2);
        }
    }
}
