using System;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class RangedNumberRequestTest
    {
        [Fact]
        public void OperandTypeIsCorrect()
        {
            // Arrange
            var expectedOperandType = typeof(int);
            var sut = new RangedNumberRequest(expectedOperandType, 1, 3);
            // Act
            var result = sut.OperandType;
            // Assert
            Assert.Equal(expectedOperandType, result);
        }

        [Fact]
        public void MinimumIsCorrect()
        {
            // Arrange
            var expectedMinimum = 1;
            var sut = new RangedNumberRequest(typeof(int), expectedMinimum, 3);
            // Act
            var result = sut.Minimum;
            // Assert
            Assert.Equal(expectedMinimum, result);
        }

        [Fact]
        public void MaximumIsCorrect()
        {
            // Arrange
            var expectedMaximum = 3;
            var sut = new RangedNumberRequest(typeof(int), 1, expectedMaximum);
            // Act
            var result = sut.Maximum;
            // Assert
            Assert.Equal(expectedMaximum, result);
        }

        [Fact]
        public void CreateWithNullOperandTypeWillThrow()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new RangedNumberRequest(null, 1, 3));
        }

        [Fact]
        public void CreateWithNullMinimumWillThrow()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new RangedNumberRequest(typeof(int), null, 3));
        }

        [Fact]
        public void CreateWithNullMaximumWillThrow()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new RangedNumberRequest(typeof(int), 1, null));
        }

        [Theory]
        [InlineData(typeof(int), 20, 10)]
        [InlineData(typeof(int), -1, -2)]
        [InlineData(typeof(decimal), 20, 10)]
        [InlineData(typeof(decimal), -1, -2)]
        [InlineData(typeof(double), 20, 10)]
        [InlineData(typeof(double), -1, -2)]
        [InlineData(typeof(long), 20, 10)]
        [InlineData(typeof(long), -1, -2)]
        public void CreateWithEqualOrBiggerMinimumThanMaximumWillThrow(Type type, object minimum, object maximum)
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new RangedNumberRequest(type, minimum, maximum));
        }

        [Theory]
        [InlineData(typeof(int), 10, 10)]
        [InlineData(typeof(decimal), 10, 10)]
        [InlineData(typeof(double), 10, 10)]
        [InlineData(typeof(long), 10, 10)]
        public void CreateWithLowerEqualToMaximunDoesNotThrow(Type type, object minimum, object maximum)
        {
            // Act
            Assert.Null(Record.Exception(() =>
                new RangedNumberRequest(type, minimum, maximum)));
            // Assert
        }

        [Theory]
        [InlineData(typeof(int), 10, 20)]
        [InlineData(typeof(int), -2, -1)]
        [InlineData(typeof(decimal), 10, 20)]
        [InlineData(typeof(decimal), -2, -1)]
        [InlineData(typeof(double), 10, 20)]
        [InlineData(typeof(double), -2, -1)]
        [InlineData(typeof(long), 10, 20)]
        [InlineData(typeof(long), -2, -1)]
        public void CreateWithLowerMinimumThanMaximumDoesNotThrow(Type type, object minimum, object maximum)
        {
            // Arrange
            // Act & assert
            Assert.Null(Record.Exception(() =>
                new RangedNumberRequest(type, minimum, maximum)));
        }

        [Fact]
        public void SutIsEquatable()
        {
            // Arrange
            // Act
            var sut = new RangedNumberRequest(typeof(int), 1, 3);
            // Assert
            Assert.IsAssignableFrom<IEquatable<RangedNumberRequest>>(sut);
        }

        [Fact]
        public void SutDoesNotEqualNullObject()
        {
            // Arrange
            var sut = new RangedNumberRequest(typeof(int), 1, 3);
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
            var sut = new RangedNumberRequest(typeof(int), 1, 3);
            RangedNumberRequest other = null;
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutDoesNotEqualAnonymousObject()
        {
            // Arrange
            var sut = new RangedNumberRequest(typeof(int), 1, 3);
            object anonymousObject = new ConcreteType();
            // Act
            var result = sut.Equals(anonymousObject);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutDoesNotEqualOtherObjectWhenOperandTypesDiffer()
        {
            // Arrange
            var sut = new RangedNumberRequest(typeof(int), 1, 3);
            object other = new RangedNumberRequest(typeof(double), 1, 3);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutDoesNotEqualOtherSutWhenOperandTypesDiffer()
        {
            // Arrange
            var sut = new RangedNumberRequest(typeof(int), 1, 3);
            var other = new RangedNumberRequest(typeof(double), 1, 3);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutDoesNotEqualOtherObjectWhenMinimumsDiffer()
        {
            // Arrange
            var sut = new RangedNumberRequest(typeof(int), 1, 3);
            object other = new RangedNumberRequest(typeof(int), 2, 3);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutDoesNotEqualOtherSutWhenMinimumsDiffer()
        {
            // Arrange
            var sut = new RangedNumberRequest(typeof(int), 1, 3);
            var other = new RangedNumberRequest(typeof(int), 2, 3);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutDoesNotEqualOtherObjectWhenMaximumsDiffer()
        {
            // Arrange
            var sut = new RangedNumberRequest(typeof(int), 1, 3);
            object other = new RangedNumberRequest(typeof(int), 1, 4);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutDoesNotEqualOtherSutWhenMaximumsDiffer()
        {
            // Arrange
            var sut = new RangedNumberRequest(typeof(int), 1, 3);
            var other = new RangedNumberRequest(typeof(int), 1, 4);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result, "Equals");
        }

        [Fact]
        public void SutEqualsOtherObjectWhenConstructorParametersEquals()
        {
            // Arrange
            Type operandType = typeof(int);
            object minimum = 1;
            object maximum = 3;
            var sut = new RangedNumberRequest(operandType, minimum, maximum);
            object other = new RangedNumberRequest(operandType, minimum, maximum);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.True(result, "Equals");
        }

        [Fact]
        public void SutEqualsOtherSutWhenConstructorParametersEquals()
        {
            // Arrange
            Type operandType = typeof(int);
            object minimum = 1;
            object maximum = 3;
            var sut = new RangedNumberRequest(operandType, minimum, maximum);
            var other = new RangedNumberRequest(operandType, minimum, maximum);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.True(result, "Equals");
        }

        [Fact]
        public void GetHashCodeWillReturnCorrectResult()
        {
            // Arrange
            Type operandType = typeof(int);
            object minimum = 1;
            object maximum = 3;
            var sut = new RangedNumberRequest(operandType, minimum, maximum);
            var expectedHashCode = operandType.GetHashCode() ^ minimum.GetHashCode() ^ maximum.GetHashCode();
            // Act
            var result = sut.GetHashCode();
            // Assert
            Assert.Equal(expectedHashCode, result);
        }

        [Fact]
        public void ToStringShouldBeOverridden()
        {
            // Arrange
            var sut = new RangedNumberRequest(typeof(long), 42, 100);

            // Act
            var stringResult = sut.ToString();

            // Assert
            Assert.Contains("Int64", stringResult);
            Assert.Contains("42", stringResult);
            Assert.Contains("100", stringResult);

        }
    }
}