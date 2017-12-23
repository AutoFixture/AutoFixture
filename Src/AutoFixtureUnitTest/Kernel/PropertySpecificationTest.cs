using System;
using System.Reflection;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class PropertySpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Arrange
            // Act
            var sut = new PropertySpecification(typeof(object), "someName");
            // Assert
            Assert.IsAssignableFrom<IRequestSpecification>(sut);
        }

        [Fact]
        [Obsolete]
        public void InitializeWithPropertyTypeAndNameShouldSetCorrespondingProperties()
        {
            // Arrange
            var type = typeof(string);
            var name = "someName";
            // Act
            var sut = new PropertySpecification(type, name);
            // Assert
#pragma warning disable 618
            Assert.Equal(type, sut.TargetType);
            Assert.Equal(name, sut.TargetName);
#pragma warning restore 618
        }

        [Fact]
        public void InitializeWithNullPropertyTypeShouldThrowArgumentNullException()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new PropertySpecification(null, "someName"));
        }

        [Fact]
        public void InitializeWithNullPropertyNameShouldThrowArgumentNullException()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new PropertySpecification(typeof(object), null));
        }

        [Fact]
        public void InitializeWithNullTargetShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => new PropertySpecification(null));
        }

        [Fact]
        public void IsSatisfiedByWithNullRequestShouldThrowArgumentNullException()
        {
            // Arrange
            var sut = new PropertySpecification(typeof(object), "someName");
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.IsSatisfiedBy(null));
        }

        [Theory]
        [InlineData("Property", "Property", true)]
        [InlineData("someName", "Property", false)]
        [InlineData("property", "Property", false)]
        public void IsSatisfiedByWithRequestForPropertyShouldReturnCorrectResult(
            string propertyName,
            string requestedName,
            bool expectedResult)
        {
            // Arrange
            var sut = new PropertySpecification(typeof(object), propertyName);
            var request = typeof(PropertyHolder<object>).GetProperty(requestedName);
            // Act
            var result = sut.IsSatisfiedBy(request);
            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void IsSatisfiedByWithRequestForReadOnlyPropertyWithSameNameShouldReturnTrue()
        {
            // Arrange
            var propertyName = "Property";
            var requestedName = "Property";
            var sut = new PropertySpecification(typeof(object), propertyName);
            var request = typeof(ReadOnlyPropertyHolder<object>).GetProperty(requestedName);
            // Act
            var result = sut.IsSatisfiedBy(request);
            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsSatisfiedByWithRequestForPropertyWithSameNameAndIncompatibleTypeShouldReturnFalse()
        {
            // Arrange
            var propertyName = "Property";
            var requestedName = "Property";
            var sut = new PropertySpecification(typeof(object), propertyName);
            var request = typeof(PropertyHolder<int>).GetProperty(requestedName);
            // Act
            var result = sut.IsSatisfiedBy(request);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsSatisfiedByWithRequestForMemberOtherThanPropertyWithSameNameShouldReturnFalse()
        {
            // Arrange
            var propertyName = "Field";
            var requestedName = "Field";
            var sut = new PropertySpecification(typeof(object), propertyName);
            var request = typeof(FieldHolder<object>).GetField(requestedName);
            // Act
            var result = sut.IsSatisfiedBy(request);
            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("aString")]
        [InlineData(9)]
        [InlineData(true)]
        [InlineData(typeof(object))]
        public void IsSatisfiedByWithInvalidRequestShouldReturnFalse(object request)
        {
            // Arrange
            var sut = new PropertySpecification(typeof(object), "someName");
            // Act
            var result = sut.IsSatisfiedBy(request);
            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsSatisfiedByReturnsCorrectResultAccordingToTarget(
            bool expected)
        {
            var prop = typeof(string).GetProperty("Length");
            var target = new DelegatingCriterion<PropertyInfo>
            {
                OnEquals = p =>
                {
                    Assert.Equal(prop, p);
                    return expected;
                }
            };
            var sut = new PropertySpecification(target);

            var actual = sut.IsSatisfiedBy(prop);

            Assert.Equal(expected, actual);
        }
    }
}
