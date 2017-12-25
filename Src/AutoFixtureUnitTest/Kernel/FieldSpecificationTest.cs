using System;
using System.Reflection;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class FieldSpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Arrange
            // Act
            var sut = new FieldSpecification(typeof(object), "someName");
            // Assert
            Assert.IsAssignableFrom<IRequestSpecification>(sut);
        }

        [Fact]
        [Obsolete]
        public void InitializeWithFieldNameShouldSetCorrespondingProperty()
        {
            // Arrange
            var type = typeof(string);
            var name = "someName";
            // Act
            var sut = new FieldSpecification(type, name);
            // Assert
#pragma warning disable 618
            Assert.Equal(type, sut.TargetType);
            Assert.Equal(name, sut.TargetName);
#pragma warning restore 618
        }

        [Fact]
        public void InitializeWithNullFieldTypeShouldThrowArgumentNullException()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new FieldSpecification(null, "someName"));
        }

        [Fact]
        public void InitializeWithNullFieldNameShouldThrowArgumentNullException()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new FieldSpecification(typeof(object), null));
        }

        [Fact]
        public void InitializeWithNullTargetThrows()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new FieldSpecification(null));
        }

        [Fact]
        public void IsSatisfiedByWithNullRequestShouldThrowArgumentNullException()
        {
            // Arrange
            var sut = new FieldSpecification(typeof(object), "someName");
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.IsSatisfiedBy(null));
        }

        [Theory]
        [InlineData("Field", "Field", true)]
        [InlineData("someName", "Field", false)]
        [InlineData("field", "Field", false)]
        public void IsSatisfiedByWithRequestForFieldShouldReturnCorrectResult(
            string fieldName,
            string requestedName,
            bool expectedResult)
        {
            // Arrange
            var sut = new FieldSpecification(typeof(object), fieldName);
            var request = typeof(FieldHolder<object>).GetField(requestedName);
            // Act
            var result = sut.IsSatisfiedBy(request);
            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void IsSatisfiedByWithRequestForFieldWithSameNameAndIncompatibleTypeShouldReturnFalse()
        {
            // Arrange
            var sut = new FieldSpecification(typeof(object), "Field");
            var request = typeof(FieldHolder<string>).GetField("Field");
            // Act
            var result = sut.IsSatisfiedBy(request);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsSatisfiedByWithRequestForMemberOtherThanFieldWithSameNameShouldReturnFalse()
        {
            // Arrange
            var sut = new FieldSpecification(typeof(object), "Property");
            var request = typeof(PropertyHolder<object>).GetProperty("Property");
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
            var sut = new FieldSpecification(typeof(object), "someName");
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
            var field = typeof(string).GetField("Empty");
            var target = new DelegatingCriterion<FieldInfo>
            {
                OnEquals = f =>
                {
                    Assert.Equal(field, f);
                    return expected;
                }
            };
            var sut = new FieldSpecification(target);

            var actual = sut.IsSatisfiedBy(field);

            Assert.Equal(expected, actual);
        }
    }
}
