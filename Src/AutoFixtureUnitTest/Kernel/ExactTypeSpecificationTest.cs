using System;
using System.Collections.Generic;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class ExactTypeSpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Arrange
            var dummyType = typeof(object);
            // Act
            var sut = new ExactTypeSpecification(dummyType);
            // Assert
            Assert.IsAssignableFrom<IRequestSpecification>(sut);
        }

        [Fact]
        public void InitializeWithNullTypeThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => new ExactTypeSpecification(null));
        }

        [Fact]
        public void TargetTypeIsCorrect()
        {
            // Arrange
            var expectedType = typeof(DayOfWeek);
            var sut = new ExactTypeSpecification(expectedType);
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
            var sut = new ExactTypeSpecification(dummyType);
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => sut.IsSatisfiedBy(null));
        }

        [Theory]
        [InlineData(typeof(object), typeof(object), true)]
        [InlineData(typeof(string), typeof(string), true)]
        [InlineData(typeof(string), typeof(int), false)]
        [InlineData(typeof(PropertyHolder<string>), typeof(FieldHolder<string>), false)]
        public void IsSatisfiedByReturnsCorrectResult(Type specType, Type requestType, bool expectedResult)
        {
            // Arrange
            var sut = new ExactTypeSpecification(specType);
            // Act
            var result = sut.IsSatisfiedBy(requestType);
            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(typeof(PropertyHolder<>), typeof(PropertyHolder<string>), true)]
        [InlineData(typeof(PropertyHolder<>), typeof(PropertyHolder<int>), true)]
        [InlineData(typeof(PropertyHolder<>), typeof(FieldHolder<string>), false)]
        [InlineData(typeof(IEnumerable<>), typeof(List<string>), false)]
        [InlineData(typeof(IEnumerable<>), typeof(IEnumerable<string>), true)]
        [InlineData(typeof(IDictionary<,>), typeof(Dictionary<string, int>), false)]
        [InlineData(typeof(IDictionary<,>), typeof(IDictionary<string, int>), true)]
        [InlineData(typeof(IEnumerable<>), typeof(string), false)]
        public void IsSatisfiedByMatchesExactlyOpenGenerics(Type specType, Type requestType, bool expectedResult)
        {
            // Arrange
            var sut = new ExactTypeSpecification(specType);
            // Act
            var result = sut.IsSatisfiedBy(requestType);
            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
