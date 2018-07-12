using System;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class RangedSequenceRequestTest
    {
        [Theory]
        [InlineData(null, 1, 3, "request")]
        [InlineData(typeof(object), -1, 3, "minLength")]
        [InlineData(typeof(object), 1, -1, "maxLength")]
        public void InitializeWithOutOfBoundaryInputThrows(object request, int minLength, int maxLength,
            string invalidArgName)
        {
            // Act & Assert
            var ex = Assert.ThrowsAny<ArgumentException>(() =>
                new RangedSequenceRequest(request, minLength, maxLength));
            Assert.Equal(invalidArgName, ex.ParamName);
        }

        [Fact]
        public void InitializeWithMaxLessThanMinThrows()
        {
            // Arrange
            var request = typeof(object);
            int max = 2;
            int min = 3;

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
                new RangedSequenceRequest(request, min, max));
            Assert.Contains("less than min", ex.Message);
        }

        [Fact]
        public void ShouldExposeTheValuesPassedToConstructor()
        {
            // Arrange
            var request = new object();
            var min = 100;
            var max = 200;

            // Act
            var sut = new RangedSequenceRequest(request, min, max);

            // Assert
            Assert.Equal(request, sut.Request);
            Assert.Equal(min, sut.MinLength);
            Assert.Equal(max, sut.MaxLength);
        }

        [Fact]
        public void SutIsEquatable()
        {
            // Act
            var sut = new RangedSequenceRequest(new object(), 10, 20);

            // Assert
            Assert.IsAssignableFrom<IEquatable<RangedSequenceRequest>>(sut);
        }

        [Fact]
        public void SutDoesNotEqualNullObject()
        {
            // Arrange
            var sut = new RangedSequenceRequest(new object(), 10, 20);
            object other = null;

            // Act
            var result = sut.Equals(other);

            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutDoesNotEqualNullSut()
        {
            // Arrange
            var sut = new RangedSequenceRequest(new object(), 10, 20);
            RangedSequenceRequest other = null;

            // Act
            var result = sut.Equals(other);

            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutDoesNotEqualAnonymousObject()
        {
            // Arrange
            var sut = new RangedSequenceRequest(new object(), 10, 20);
            object anonymousObject = new ConcreteType();

            // Act
            var result = sut.Equals(anonymousObject);

            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutDoesNotEqualOtherObjectWhenRequestsDifferButRangeMatches()
        {
            // Arrange
            var min = 10;
            var max = 10;
            var sut = new RangedSequenceRequest(new object(), min, max);
            object other = new RangedSequenceRequest(new object(), min, max);

            // Act
            var result = sut.Equals(other);

            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutDoesNotEqualOtherObjectWhenRangeDifferButRequestMatches()
        {
            // Arrange
            var request = new object();
            var sut = new RangedSequenceRequest(request, 10, 20);
            object other = new RangedSequenceRequest(request, 1, 30);

            // Act
            var result = sut.Equals(other);

            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutEqualsOtherObjectWhenBothRequestsAndRangeMatch()
        {
            // Arrange
            var request = new object();
            var min = 10;
            var max = 20;
            var sut = new RangedSequenceRequest(request, min, max);
            object other = new RangedSequenceRequest(request, min, max);

            // Act
            var result = sut.Equals(other);

            // Assert
            Assert.True(result, "Equals");
        }

        [Fact]
        public void SutEqualsOtherSutWhenBothRequestsAndRangeMatch()
        {
            // Arrange
            var request = new object();
            var min = 10;
            var max = 20;
            var sut = new RangedSequenceRequest(request, min, max);
            var other = new RangedSequenceRequest(request, min, max);

            // Act
            var result = sut.Equals(other);

            // Assert
            Assert.True(result, "Equals");
        }

        [Theory]
        [InlineData(typeof(string), 10, 20)]
        [InlineData(typeof(object), 5, 20)]
        [InlineData(typeof(object), 5, 50)]
        public void GetHashCodeChangesWhenAnyMemberDiffers(object request, int minLength, int maxLength)
        {
            // Arrange
            var etalonHashCode = new RangedSequenceRequest(typeof(object), 10, 20).GetHashCode();
            var sut = new RangedSequenceRequest(request, minLength, maxLength);

            // Act
            var result = sut.GetHashCode();

            // Assert
            Assert.NotEqual(etalonHashCode, result);
        }
    }
}
