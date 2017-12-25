using System;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class ParameterSpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Arrange
            // Act
            var sut = new ParameterSpecification(typeof(object), "someName");
            // Assert
            Assert.IsAssignableFrom<IRequestSpecification>(sut);
        }

        [Fact]
        [Obsolete]
        public void InitializeWithParameterNameShouldSetCorrespondingProperty()
        {
            // Arrange
            var type = typeof(object);
            var name = "someName";
            // Act
            var sut = new ParameterSpecification(type, name);
            // Assert
#pragma warning disable 618
            Assert.Equal(type, sut.TargetType);
            Assert.Equal(name, sut.TargetName);
#pragma warning restore 618
        }

        [Fact]
        public void InitializeWithNullParameterTypeShouldThrowArgumentNullException()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new ParameterSpecification(null, "someName"));
        }

        [Fact]
        public void InitializeWithNullParameterNameShouldThrowArgumentNullException()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new ParameterSpecification(typeof(object), null));
        }

        [Fact]
        public void InitializeWithNullTargetThrows()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new ParameterSpecification(null));
        }

        [Fact]
        public void IsSatisfiedByWithNullRequestShouldThrowArgumentNullException()
        {
            // Arrange
            var sut = new ParameterSpecification(typeof(object), "someName");
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.IsSatisfiedBy(null));
        }

        [Theory]
        [InlineData("parameter", "parameter", true)]
        [InlineData("someName", "parameter", false)]
        [InlineData("Parameter", "parameter", false)]
        public void IsSatisfiedByWithRequestForParameterShouldReturnCorrectResult(
            string parameterName,
            string requestedName,
            bool expectedResult)
        {
            // Arrange
            var sut = new ParameterSpecification(typeof(object), parameterName);
            var request = typeof(SingleParameterType<object>)
                          .GetConstructor(new[] { typeof(object) })
                          .GetParameters()
                          .Single(p => p.Name == requestedName);
            // Act
            var result = sut.IsSatisfiedBy(request);
            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void IsSatisfiedByWithRequestForParameterWithSameNameAndIncompatibleTypeShouldReturnFalse()
        {
            // Arrange
            var parameterName = "parameter";
            var requestedName = "parameter";
            var sut = new ParameterSpecification(typeof(object), parameterName);
            var request = typeof(SingleParameterType<int>)
                          .GetConstructor(new[] { typeof(int) })
                          .GetParameters()
                          .Single(p => p.Name == requestedName);
            // Act
            var result = sut.IsSatisfiedBy(request);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsSatisfiedByWithRequestForMemberOtherThanParameterWithSameNameShouldReturnFalse()
        {
            // Arrange
            var parameterName = "Parameter";
            var requestedName = "Parameter";
            var sut = new ParameterSpecification(typeof(object), parameterName);
            var request = typeof(SingleParameterType<object>).GetProperty(requestedName);
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
            var sut = new ParameterSpecification(typeof(object), "someName");
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
            var parameter =
                typeof(string).GetMethod("Contains").GetParameters().First();
            var target = new DelegatingCriterion<ParameterInfo>
            {
                OnEquals = f =>
                {
                    Assert.Equal(parameter, f);
                    return expected;
                }
            };
            var sut = new ParameterSpecification(target);

            var actual = sut.IsSatisfiedBy(parameter);

            Assert.Equal(expected, actual);
        }
    }
}
