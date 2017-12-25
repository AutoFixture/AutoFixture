using System;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.AutoNSubstitute.UnitTest
{
    public class SubstituteRequestTest
    {
        [Fact]
        public void TargetTypeReturnsValueSpecifiedInConstructor()
        {
            // Arrange
            var expectedType = typeof(IInterface);
            // Act
            var sut = new SubstituteRequest(expectedType);
            // Assert
            Assert.Same(expectedType, sut.TargetType);
        }

        [Fact]
        public void ConstructorThrowsArgumentNullExceptionWhenTargetTypeIsNull()
        {
            // Arrange
            // Act
            var e = Assert.Throws<ArgumentNullException>(() => new SubstituteRequest(null));
            // Assert
            Assert.Equal("targetType", e.ParamName);
        }

        [Fact]
        public void ToStringOutputContainsTypeName()
        {
            // Arrange
            var type = typeof(string);
            var typeName = type.ToString();

            var sut = new SubstituteRequest(type);

            // Act
            var actualOutput = sut.ToString();

            // Assert
            Assert.Contains(typeName, actualOutput);
        }
    }
}
