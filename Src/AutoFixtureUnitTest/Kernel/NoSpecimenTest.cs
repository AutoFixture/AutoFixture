using System;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class NoSpecimenTest
    {
        [Fact]
        [Obsolete]
        public void DefaultConstructorWillSetRequestToNull()
        {
            // Arrange
            var sut = new NoSpecimen();
            // Act
#pragma warning disable 618
            var result = sut.Request;
#pragma warning restore 618
            // Assert
            Assert.Null(result);
        }

        [Fact]
        [Obsolete]
        public void CreateWithNullRequestWillSetCorrectRequest()
        {
            // Arrange
            // Act
#pragma warning disable 618
            var sut = new NoSpecimen(null);
            // Assert
            Assert.Null(sut.Request);
#pragma warning restore 618
        }

        [Fact]
        [Obsolete]
        public void RequestWillMatchConstructorArgument()
        {
            // Arrange
            var expectedRequest = new object();
#pragma warning disable 618
            var sut = new NoSpecimen(expectedRequest);
            // Act
            var result = sut.Request;
#pragma warning restore 618
            // Assert
            Assert.Equal(expectedRequest, result);
        }

        [Fact]
        public void SutIsEquatable()
        {
            // Arrange
            // Act
            var sut = new NoSpecimen();
            // Assert
            Assert.IsAssignableFrom<IEquatable<NoSpecimen>>(sut);
        }

        [Fact]
        public void SutDoesNotEqualNullObject()
        {
            // Arrange
            var sut = new NoSpecimen();
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
            var sut = new NoSpecimen();
            NoSpecimen other = null;
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutDoesNotEqualAnonymousObject()
        {
            // Arrange
            var sut = new NoSpecimen();
            var anonymousObject = new object();
            // Act
            var result = sut.Equals(anonymousObject);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        [Obsolete]
        public void SutDoesNotEqualOtherObjectWhenSutRequestIsNull()
        {
            // Arrange
            var sut = new NoSpecimen();
#pragma warning disable 618
            object other = new NoSpecimen(new object());
#pragma warning restore 618
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        [Obsolete]
        public void SutDoesNotEqualOtherSutWhenSutRequestIsNull()
        {
            // Arrange
            var sut = new NoSpecimen();
#pragma warning disable 618
            var other = new NoSpecimen(new object());
#pragma warning restore 618
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        [Obsolete]
        public void SutDoesNotEqualOtherObjectWhenOtherRequestIsNull()
        {
            // Arrange
#pragma warning disable 618
            var sut = new NoSpecimen(new object());
#pragma warning restore 618
            object other = new NoSpecimen();
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        [Obsolete]
        public void SutDoesNotEqualOtherSutWhenOtherRequestIsNull()
        {
            // Arrange
#pragma warning disable 618
            var sut = new NoSpecimen(new object());
#pragma warning restore 618
            var other = new NoSpecimen();
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        [Obsolete]
        public void SutDoesNotEqualOtherObjectWhenRequestsDiffer()
        {
            // Arrange
#pragma warning disable 618
            var sut = new NoSpecimen(new object());
            object other = new NoSpecimen(new object());
#pragma warning restore 618
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        [Obsolete]
        public void SutDoesNotEqualOtherSutWhenRequestsDiffer()
        {
            // Arrange
#pragma warning disable 618
            var sut = new NoSpecimen(new object());
            var other = new NoSpecimen(new object());
#pragma warning restore 618
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutEqualsOtherObjectWhenBothRequestsAreNull()
        {
            // Arrange
            var sut = new NoSpecimen();
            object other = new NoSpecimen();
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.True(result, "Equals");
        }

        [Fact]
        public void SutEqualsOtherSutWhenBothRequestsAreNull()
        {
            // Arrange
            var sut = new NoSpecimen();
            var other = new NoSpecimen();
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.True(result, "Equals");
        }

        [Fact]
        [Obsolete]
        public void SutEqualsOtherObjectWhenRequestsAreEqual()
        {
            // Arrange
            var request = new object();
#pragma warning disable 618
            var sut = new NoSpecimen(request);
            object other = new NoSpecimen(request);
#pragma warning restore 618
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.True(result, "Equals");
        }

        [Fact]
        [Obsolete]
        public void SutEqualsOtherSutWhenRequestsAreEqual()
        {
            // Arrange
            var request = new object();
#pragma warning disable 618
            var sut = new NoSpecimen(request);
            var other = new NoSpecimen(request);
#pragma warning restore 618
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.True(result, "Equals");
        }

        [Fact]
        public void GetHashCodeWhenRequestIsNullWillReturnCorrectResult()
        {
            // Arrange
            var sut = new NoSpecimen();
            // Act
            var result = sut.GetHashCode();
            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        [Obsolete]
        public void GetHashCodeWhenRequestIsNotNullWillReturnCorrectResult()
        {
            // Arrange
            var request = new object();
#pragma warning disable 618
            var sut = new NoSpecimen(request);
#pragma warning restore 618
            // Act
            var result = sut.GetHashCode();
            // Assert
            var expectedHashCode = request.GetHashCode();
            Assert.Equal(expectedHashCode, result);
        }
    }
}
