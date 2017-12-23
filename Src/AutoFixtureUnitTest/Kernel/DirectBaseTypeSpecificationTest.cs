using System;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class DirectBaseTypeSpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Arrange
            // Act
            var sut = new DirectBaseTypeSpecification(typeof(object));
            // Assert
            Assert.IsAssignableFrom<IRequestSpecification>(sut);
        }

        [Fact]
        public void InitializeWithTargetTypeShouldSetCorrespondingProperty()
        {
            // Arrange
            var targetType = typeof(object);
            // Act
            var sut = new DirectBaseTypeSpecification(targetType);
            // Assert
            Assert.Equal(targetType, sut.TargetType);
        }

        [Fact]
        public void InitializeWithNullTargetTypeShouldThrowArgumentNullException()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new DirectBaseTypeSpecification(null));
        }

        [Fact]
        public void IsSatisfiedByWithNullRequestShouldThrowArgumentNullException()
        {
            // Arrange
            var sut = new DirectBaseTypeSpecification(typeof(object));
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.IsSatisfiedBy(null));
        }

        [Fact]
        public void IsSatisfiedByWithRequestForDirectBaseTypeShouldReturnTrue()
        {
            // Arrange
            var targetType = typeof(ConcreteType);
            var requestedType = typeof(AbstractType);
            var sut = new DirectBaseTypeSpecification(targetType);
            // Act
            var result = sut.IsSatisfiedBy(requestedType);
            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsSatisfiedByWithRequestForSameTypeShouldReturnTrue()
        {
            // Arrange
            var targetType = typeof(ConcreteType);
            var requestedType = typeof(ConcreteType);
            var sut = new DirectBaseTypeSpecification(targetType);
            // Act
            var result = sut.IsSatisfiedBy(requestedType);
            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsSatisfiedByWithRequestForIndirectBaseTypeShouldReturnFalse()
        {
            // Arrange
            var targetType = typeof(ConcreteType);
            var requestedType = typeof(object);
            var sut = new DirectBaseTypeSpecification(targetType);
            // Act
            var result = sut.IsSatisfiedBy(requestedType);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsSatisfiedByWithRequestForIncompatibleTypeShouldReturnFalse()
        {
            // Arrange
            var targetType = typeof(ConcreteType);
            var requestedType = typeof(string);
            var sut = new DirectBaseTypeSpecification(targetType);
            // Act
            var result = sut.IsSatisfiedBy(requestedType);
            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("aString")]
        [InlineData(9)]
        [InlineData(true)]
        public void IsSatisfiedByWithInvalidRequestShouldReturnFalse(object request)
        {
            // Arrange
            var sut = new DirectBaseTypeSpecification(typeof(ConcreteType));
            // Act
            var result = sut.IsSatisfiedBy(request);
            // Assert
            Assert.False(result);
        }
    }
}
