using System;
using AutoFixture.DataAnnotations;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.DataAnnotations
{
    public class RangedRequestTest
    {
        [Theory]
        [InlineData(null, typeof(int), 1, 2)]
        [InlineData(typeof(int), null, 1, 2)]
        [InlineData(typeof(int), typeof(int), null, 2)]
        [InlineData(typeof(int), typeof(int), 1, null)]
        public void ShouldFailWithArgumentExceptionForNullArguments(Type memberType, Type operandType, object min, object max)
        {
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new RangedRequest(memberType, operandType, min, max));
        }

        [Fact]
        public void PropertiesShouldReturnPassedToConstructorValues()
        {
            // Arrange
            var memberType = typeof(object);
            var operandType = typeof(int);
            var min = 42;
            var max = 100;

            // Act
            var sut = new RangedRequest(memberType, operandType, min, max);

            // Assert
            Assert.Equal(memberType, sut.MemberType);
            Assert.Equal(operandType, sut.OperandType);
            Assert.Equal(min, sut.Minimum);
            Assert.Equal(max, sut.Maximum);
        }

        [Fact]
        public void SutIsEquatable()
        {
            // Arrange
            var sut = new RangedRequest(typeof(int), typeof(int), 1, 2);

            // Act & assert
            Assert.IsAssignableFrom<IEquatable<RangedRequest>>(sut);
        }

        [Fact]
        public void SutIsNotEqualNullObject()
        {
            // Arrange
            var sut = new RangedRequest(typeof(int), typeof(int), 1, 2);
            object nullObj = null;

            // Act
            var areEqual = sut.Equals(nullObj);

            // Assert
            Assert.False(areEqual);
        }

        [Fact]
        public void SutIsNotEqualNullSut()
        {
            // Arrange
            var sut = new RangedRequest(typeof(int), typeof(int), 1, 2);
            RangedRequest nullSut = null;

            // Act
            var areEqual = sut.Equals(nullSut);

            // Assert
            Assert.False(areEqual);
        }

        [Fact]
        public void SutIsNotEqualOtherTypeObject()
        {
            // Arrange
            var sut = new RangedRequest(typeof(int), typeof(int), 1, 2);
            var otherType = new object();

            // Act
            var areEqual = sut.Equals(otherType);

            // Assert
            Assert.False(areEqual);
        }

        [Theory]
        [InlineData(typeof(int), typeof(int), 1, 3)]
        [InlineData(typeof(long), typeof(long), 1, 3)]
        [InlineData(typeof(long), typeof(int), 2, 3)]
        [InlineData(typeof(long), typeof(int), 1, 4)]
        public void SutIsNotEqualOtherSutWhenAnyMemberDiffers(Type memberType, Type operandType, object min, object max)
        {
            // Arrange
            var sut = new RangedRequest(typeof(long), typeof(int), 1, 3);
            RangedRequest other = new RangedRequest(memberType, operandType, min, max);

            // Act
            bool areEqual = sut.Equals(other);
            // Assert
            Assert.False(areEqual);
        }

        [Theory]
        [InlineData(typeof(int), typeof(int), 1, 3)]
        [InlineData(typeof(long), typeof(long), 1, 3)]
        [InlineData(typeof(long), typeof(int), 2, 3)]
        [InlineData(typeof(long), typeof(int), 1, 4)]
        public void SutIsNotEqualOtherObjectWhenAnyMemberDiffers(Type memberType, Type operandType, object min, object max)
        {
            // Arrange
            var sut = new RangedRequest(typeof(long), typeof(int), 1, 3);
            object other = new RangedRequest(memberType, operandType, min, max);

            // Act
            bool areEqual = sut.Equals(other);
            // Assert
            Assert.False(areEqual);
        }

        [Fact]
        public void SutIsEqualOtherSutIfAllMembersEqual()
        {
            // Arrange
            var memberType = typeof(decimal);
            var operandType = typeof(int);
            var min = 1;
            var max = 3;

            var sut = new RangedRequest(memberType, operandType, min, max);
            RangedRequest other = new RangedRequest(memberType, operandType, min, max);

            // Act
            bool areEqual = sut.Equals(other);

            // Assert
            Assert.True(areEqual);
        }

        [Fact]
        public void SutIsEqualOtherObjectIfAllMembersEqual()
        {
            // Arrange
            var memberType = typeof(decimal);
            var operandType = typeof(int);
            var min = 1;
            var max = 3;

            var sut = new RangedRequest(memberType, operandType, min, max);
            object other = new RangedRequest(memberType, operandType, min, max);

            // Act
            bool areEqual = sut.Equals(other);

            // Assert
            Assert.True(areEqual);
        }

        [Theory]
        [InlineData(typeof(int), typeof(int), 1, 3)]
        [InlineData(typeof(long), typeof(long), 1, 3)]
        [InlineData(typeof(long), typeof(int), 2, 3)]
        [InlineData(typeof(long), typeof(int), 1, 4)]
        public void HashCodeIsDifferentWhenAnyMemberChanges(Type memberType, Type operandType, object min, object max)
        {
            // Arrange
            var etalonHashCode = new RangedRequest(typeof(long), typeof(int), 1, 3).GetHashCode();
            var sut = new RangedRequest(memberType, operandType, min, max);

            // Act
            var newHashCode = sut.GetHashCode();

            // Assert
            Assert.NotEqual(etalonHashCode, newHashCode);
        }

        [Fact]
        public void HashCodeIsSameWhenAllMembersAreSame()
        {
            // Arrange
            var memberType = typeof(decimal);
            var operandType = typeof(int);
            var min = 1;
            var max = 3;

            var sut1 = new RangedRequest(memberType, operandType, min, max);
            var sut2 = new RangedRequest(memberType, operandType, min, max);

            // Act
            var hash1 = sut1.GetHashCode();
            var hash2 = sut2.GetHashCode();

            // Assert
            Assert.Equal(hash1, hash2);
        }

        [Theory]
        [InlineData((int)42)]
        [InlineData((uint)42)]
        [InlineData((long)42)]
        [InlineData((ulong)42)]
        [InlineData((double)42.0)]
        [InlineData((float)42.0f)]
        [InlineData("42")]
        public void ShouldCorrectlyConvertMinimum(object bounaryValue)
        {
            // Arrange
            var sut = new RangedRequest(typeof(int), typeof(int), bounaryValue, int.MaxValue);

            // Act
            var convertedValue = sut.GetConvertedMinimum(typeof(int));

            // Assert
            Assert.Equal(42, convertedValue);
        }

        [Theory]
        [InlineData((int)42)]
        [InlineData((uint)42)]
        [InlineData((long)42)]
        [InlineData((ulong)42)]
        [InlineData((double)42.0)]
        [InlineData((float)42.0f)]
        [InlineData("42")]
        public void ShouldCorrectlyConvertMaximum(object bounaryValue)
        {
            // Arrange
            var sut = new RangedRequest(typeof(int), typeof(int), 0, bounaryValue);

            // Act
            var convertedValue = sut.GetConvertedMaximum(typeof(int));

            // Assert
            Assert.Equal(42, convertedValue);
        }

        [Fact]
        public void FailsWithMeaningfulExceptionWhenMinimumCannotBeConvertedWithoutOverflow()
        {
            // Arrange
            double valueWithOverflow = (double)long.MaxValue;

            var sut = new RangedRequest(typeof(long), typeof(double), valueWithOverflow, double.MaxValue);

            // Act & assert
            var actualEx = Assert.Throws<OverflowException>(() =>
                sut.GetConvertedMinimum(typeof(long)));
            Assert.Contains("To solve the issue", actualEx.Message);
        }

        [Fact]
        public void FailsWithMeaningfulExceptionWhenMaximumCannotBeConvertedWithoutOverflow()
        {
            // Arrange
            double valueWithOverflow = (double)long.MaxValue;

            var sut = new RangedRequest(typeof(long), typeof(double), 0, valueWithOverflow);

            // Act & assert
            var actualEx = Assert.Throws<OverflowException>(() =>
                sut.GetConvertedMaximum(typeof(long)));
            Assert.Contains("To solve the issue", actualEx.Message);
        }

        [Fact]
        public void ShouldConvertLiteralEnumValues()
        {
            // Arrange
            var sut = new RangedRequest(
                typeof(EnumType), typeof(string), nameof(EnumType.First), nameof(EnumType.Third));

            // Act
            var convertedMin = sut.GetConvertedMinimum(typeof(EnumType));
            var convertedMax = sut.GetConvertedMaximum(typeof(EnumType));

            // Assert
            Assert.Equal(EnumType.First, convertedMin);
            Assert.Equal(EnumType.Third, convertedMax);
        }

        [Fact]
        public void ToStringShouldBeOverridden()
        {
            // Arrange
            var sut = new RangedRequest(typeof(long), typeof(int), 42, 100);

            // Act
            var stringResult = sut.ToString();

            // Assert
            Assert.Contains("Int32", stringResult);
            Assert.Contains("Int64", stringResult);
            Assert.Contains("42", stringResult);
            Assert.Contains("100", stringResult);
        }
    }
}