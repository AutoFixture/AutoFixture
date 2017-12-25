using System;
using System.Reflection;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class SeededRequestTest
    {
        [Fact]
        public void SeedIsCorrect()
        {
            // Arrange
            var expectedSeed = "Anonymous value";
            var sut = new SeededRequest(typeof(string), expectedSeed);
            // Act
            var result = sut.Seed;
            // Assert
            Assert.Equal(expectedSeed, result);
        }

        [Fact]
        public void CreateWithNullRequestWillThrow()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => new SeededRequest(null, new object()));
            // Assert (expected exception)
        }

        [Fact]
        public void RequestIsCorrect()
        {
            // Arrange
            var expectedRequest = new object();
            var sut = new SeededRequest(expectedRequest, "Anonymous value");
            // Act
            var result = sut.Request;
            // Assert
            Assert.Equal(expectedRequest, result);
        }

        [Fact]
        public void SutIsEquatable()
        {
            // Arrange
            // Act
            var sut = new SeededRequest(typeof(decimal), 1);
            // Assert
            Assert.IsAssignableFrom<IEquatable<SeededRequest>>(sut);
        }

        [Fact]
        public void SutDoesNotEqualNullObject()
        {
            // Arrange
            var sut = new SeededRequest(typeof(DateTime), new DateTime(103029));
            object other = null;
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SutDoesNotEqualNullSut()
        {
            // Arrange
            var sut = new SeededRequest(typeof(TimeSpan), new object());
            SeededRequest other = null;
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SutDoesNotEqualAnonymousObject()
        {
            // Arrange
            var sut = new SeededRequest(new object(), "Anonymous value");
            object anonymousObject = new int[0];
            // Act
            var result = sut.Equals(anonymousObject);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SutDoesNotEqualOtherObjectWhenRequestsDiffer()
        {
            // Arrange
            var anonymousValue = 1;
            var sut = new SeededRequest(new object(), anonymousValue);
            object other = new SeededRequest(typeof(TimeSpan), anonymousValue);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutDoesNotEqualOtherSutWhenRequestsDiffer()
        {
            // Arrange
            var anonymousValue = 1;
            var sut = new SeededRequest(new object(), anonymousValue);
            var other = new SeededRequest(typeof(TimeSpan), anonymousValue);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutDoesNotEqualOtherObjectWhenSeedsDiffer()
        {
            // Arrange
            var anonymousRequest = new object();
            var sut = new SeededRequest(anonymousRequest, 98);
            object other = new SeededRequest(anonymousRequest, "Anonymous value");
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutDoesNotEqualOtherSutWhenSeedsDiffer()
        {
            // Arrange
            var anonymousRequest = 1;
            var sut = new SeededRequest(anonymousRequest, 98);
            var other = new SeededRequest(anonymousRequest, "Anonymous value");
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutDoesNotEqualOtherObjectWhenSutSeedIsNull()
        {
            // Arrange
            var anonymousRequest = string.Empty;
            var sut = new SeededRequest(anonymousRequest, null);
            object other = new SeededRequest(anonymousRequest, 2.9f);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutDoesNotEqualOtherSutWhenSutSeedIsNull()
        {
            // Arrange
            var anonymousRequest = typeof(float);
            var sut = new SeededRequest(anonymousRequest, null);
            var other = new SeededRequest(anonymousRequest, 2.9f);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutDoesNotEqualOtherObjectWhenOtherSeedIsNull()
        {
            // Arrange
            var anonymousRequest = typeof(Buffer);
            var sut = new SeededRequest(anonymousRequest, new object());
            object other = new SeededRequest(anonymousRequest, null);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutDoesNotEqualOtherSutWhenOtherSeedIsNull()
        {
            // Arrange
            var anonymousRequest = typeof(Buffer);
            var sut = new SeededRequest(anonymousRequest, new object());
            var other = new SeededRequest(anonymousRequest, null);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutEqualsOtherSutWhenRequestAndSeedEquals()
        {
            // Arrange
            var request = typeof(object);
            var seed = new ConcreteType();
            var sut = new SeededRequest(request, seed);
            object other = new SeededRequest(request, seed);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.True(result);
        }

        [Fact]
        public void SutEqualsOtherSutWhenRequestsAndSeedEquals()
        {
            // Arrange
            var request = typeof(object);
            var seed = new ConcreteType();
            var sut = new SeededRequest(request, seed);
            var other = new SeededRequest(request, seed);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.True(result);
        }

        [Fact]
        public void SutEqualsOtherObjectWhenRequestsAreEqualAndSeedsAreNull()
        {
            // Arrange
            var request = typeof(WeakReference);
            var sut = new SeededRequest(request, null);
            object other = new SeededRequest(request, null);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.True(result);
        }

        [Fact]
        public void SutEqualsOtherSutWhenRequestsAreEqualAndSeedsAreNull()
        {
            // Arrange
            var request = typeof(WeakReference);
            var sut = new SeededRequest(request, null);
            var other = new SeededRequest(request, null);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GetHashCodeWillReturnCorrectResultWhenSeedIsNull()
        {
            // Arrange
            var request = typeof(Version);
            var sut = new SeededRequest(request, null);
            var expectedHashCode = request.GetHashCode();
            // Act
            var result = sut.GetHashCode();
            // Assert
            Assert.Equal(expectedHashCode, result);
        }

        [Fact]
        public void GetHashCodeWillReturnCorrectResult()
        {
            // Arrange
            var request = typeof(object);
            var value = Missing.Value;
            var sut = new SeededRequest(request, value);
            var expectedHashCode = request.GetHashCode() ^ value.GetHashCode();
            // Act
            var result = sut.GetHashCode();
            // Assert
            Assert.Equal(expectedHashCode, result);
        }
    }
}
