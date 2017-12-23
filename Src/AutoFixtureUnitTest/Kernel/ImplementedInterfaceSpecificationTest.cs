using System;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class ImplementedInterfaceSpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Arrange
            // Act
            var sut = new ImplementedInterfaceSpecification(typeof(object));
            // Assert
            Assert.IsAssignableFrom<IRequestSpecification>(sut);
        }

        [Fact]
        public void InitializeWithTargetTypeShouldSetCorrespondingProperty()
        {
            // Arrange
            var targetType = typeof(object);
            // Act
            var sut = new ImplementedInterfaceSpecification(targetType);
            // Assert
            Assert.Equal(targetType, sut.TargetType);
        }

        [Fact]
        public void InitializeWithNullTargetTypeShouldThrowArgumentNullException()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new ImplementedInterfaceSpecification(null));
        }

        [Fact]
        public void IsSatisfiedByWithNullRequestShouldThrowArgumentNullException()
        {
            // Arrange
            var sut = new ImplementedInterfaceSpecification(typeof(object));
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.IsSatisfiedBy(null));
        }

        [Fact]
        public void IsSatisfiedByWithRequestForSameTypeShouldReturnTrue()
        {
            // Arrange
            var targetType = typeof(NoopInterfaceImplementer);
            var requestedType = typeof(NoopInterfaceImplementer);
            var sut = new ImplementedInterfaceSpecification(targetType);
            // Act
            var result = sut.IsSatisfiedBy(requestedType);
            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsSatisfiedByWithRequestForImplementedInterfaceShouldReturnTrue()
        {
            // Arrange
            var targetType = typeof(NoopInterfaceImplementer);
            var requestedType = typeof(IInterface);
            var sut = new ImplementedInterfaceSpecification(targetType);
            // Act
            var result = sut.IsSatisfiedBy(requestedType);
            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsSatisfiedByWithRequestForNotImplementedInterfaceShouldReturnFalse()
        {
            // Arrange
            var targetType = typeof(NoopInterfaceImplementer);
            var requestedType = typeof(IDisposable);
            var sut = new ImplementedInterfaceSpecification(targetType);
            // Act
            var result = sut.IsSatisfiedBy(requestedType);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsSatisfiedByWithRequestForNonInterfaceTypeShouldReturnFalse()
        {
            // Arrange
            var targetType = typeof(NoopInterfaceImplementer);
            var requestedType = typeof(string);
            var sut = new ImplementedInterfaceSpecification(targetType);
            // Act
            var result = sut.IsSatisfiedBy(requestedType);
            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("aString")]
        [InlineData(3)]
        [InlineData(false)]
        public void IsSatisfiedByWithInvalidRequestShouldReturnFalse(object request)
        {
            // Arrange
            var targetType = typeof(NoopInterfaceImplementer);
            var sut = new ImplementedInterfaceSpecification(targetType);
            // Act
            var result = sut.IsSatisfiedBy(request);
            // Assert
            Assert.False(result);
        }
    }
}
