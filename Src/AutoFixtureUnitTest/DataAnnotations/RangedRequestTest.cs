using System;
using AutoFixture.DataAnnotations;
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
            // Exercise system and Verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new RangedRequest(memberType, operandType, min, max));

            // Teardown
        }

        [Fact]
        public void PropertiesShouldReturnPassedToConstructorValues()
        {
            // Fixture setup
            var memberType = typeof(object);
            var operandType = typeof(int);
            var min = 42;
            var max = 100;

            // Exercise system
            var sut = new RangedRequest(memberType, operandType, min, max);

            // Verify outcome
            Assert.Equal(memberType, sut.MemberType);
            Assert.Equal(operandType, sut.OperandType);
            Assert.Equal(min, sut.Minimum);
            Assert.Equal(max, sut.Maximum);

            // Teardown
        }

        [Fact]
        public void SutIsEquatable()
        {
            // Fixture setup
            var sut = new RangedRequest(typeof(int), typeof(int), 1, 2);

            // Exercise system and Verify outcome
            Assert.IsAssignableFrom<IEquatable<RangedRequest>>(sut);
            // Teardown
        }

        [Fact]
        public void SutIsNotEqualNullObject()
        {
            // Fixture setup
            var sut = new RangedRequest(typeof(int), typeof(int), 1, 2);
            object nullObj = null;

            // Exercise system
            var areEqual = sut.Equals(nullObj);

            // Verify outcome
            Assert.False(areEqual);
            // Teardown
        }

        [Fact]
        public void SutIsNotEqualNullSut()
        {
            // Fixture setup
            var sut = new RangedRequest(typeof(int), typeof(int), 1, 2);
            RangedRequest nullSut = null;

            // Exercise system
            var areEqual = sut.Equals(nullSut);

            // Verify outcome
            Assert.False(areEqual);
            // Teardown
        }

        [Fact]
        public void SutIsNotEqualOtherTypeObject()
        {
            // Fixture setup
            var sut = new RangedRequest(typeof(int), typeof(int), 1, 2);
            var otherType = new object();

            // Exercise system
            var areEqual = sut.Equals(otherType);

            // Verify outcome
            Assert.False(areEqual);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(int), typeof(int), 1, 3)]
        [InlineData(typeof(long), typeof(long), 1, 3)]
        [InlineData(typeof(long), typeof(int), 2, 3)]
        [InlineData(typeof(long), typeof(int), 1, 4)]
        public void SutIsNotEqualOtherSutWhenAnyMemberDiffers(Type memberType, Type operandType, object min, object max)
        {
            // Fixture setup
            var sut = new RangedRequest(typeof(long), typeof(int), 1, 3);
            RangedRequest other = new RangedRequest(memberType, operandType, min, max);

            // Exercise system
            bool areEqual = sut.Equals(other);
            // Verify outcome
            Assert.False(areEqual);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(int), typeof(int), 1, 3)]
        [InlineData(typeof(long), typeof(long), 1, 3)]
        [InlineData(typeof(long), typeof(int), 2, 3)]
        [InlineData(typeof(long), typeof(int), 1, 4)]
        public void SutIsNotEqualOtherObjectWhenAnyMemberDiffers(Type memberType, Type operandType, object min, object max)
        {
            // Fixture setup
            var sut = new RangedRequest(typeof(long), typeof(int), 1, 3);
            object other = new RangedRequest(memberType, operandType, min, max);

            // Exercise system
            bool areEqual = sut.Equals(other);
            // Verify outcome
            Assert.False(areEqual);
            // Teardown
        }

        [Fact]
        public void SutIsEqualOtherSutIfAllMembersEqual()
        {
            // Fixture setup
            var memberType = typeof(decimal);
            var operandType = typeof(int);
            var min = 1;
            var max = 3;

            var sut = new RangedRequest(memberType, operandType, min, max);
            RangedRequest other = new RangedRequest(memberType, operandType, min, max);

            // Exercise system
            bool areEqual = sut.Equals(other);

            // Verify outcome
            Assert.True(areEqual);
            // Teardown
        }

        [Fact]
        public void SutIsEqualOtherObjectIfAllMembersEqual()
        {
            // Fixture setup
            var memberType = typeof(decimal);
            var operandType = typeof(int);
            var min = 1;
            var max = 3;

            var sut = new RangedRequest(memberType, operandType, min, max);
            object other = new RangedRequest(memberType, operandType, min, max);

            // Exercise system
            bool areEqual = sut.Equals(other);

            // Verify outcome
            Assert.True(areEqual);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(int), typeof(int), 1, 3)]
        [InlineData(typeof(long), typeof(long), 1, 3)]
        [InlineData(typeof(long), typeof(int), 2, 3)]
        [InlineData(typeof(long), typeof(int), 1, 4)]
        public void HashCodeIsDifferentWhenAnyMemberChanges(Type memberType, Type operandType, object min, object max)
        {
            // Fixture setup
            var etalonHashCode = new RangedRequest(typeof(long), typeof(int), 1, 3).GetHashCode();
            var sut = new RangedRequest(memberType, operandType, min, max);

            // Exercise system
            var newHashCode = sut.GetHashCode();

            // Verify outcome
            Assert.NotEqual(etalonHashCode, newHashCode);
            // Teardown
        }

        [Fact]
        public void HashCodeIsSameWhenAllMembersAreSame()
        {
            // Fixture setup
            var memberType = typeof(decimal);
            var operandType = typeof(int);
            var min = 1;
            var max = 3;

            var sut1 = new RangedRequest(memberType, operandType, min, max);
            var sut2 = new RangedRequest(memberType, operandType, min, max);

            // Exercise system
            var hash1 = sut1.GetHashCode();
            var hash2 = sut2.GetHashCode();

            // Verify outcome
            Assert.Equal(hash1, hash2);
            // Teardown
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
            // Fixture setup
            var sut = new RangedRequest(typeof(int), typeof(int), bounaryValue, int.MaxValue);

            // Exercise system
            var convertedValue = sut.GetConvertedMinimum(typeof(int));

            // Verify outcome
            Assert.Equal(42, convertedValue);
            // Teardown
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
            // Fixture setup
            var sut = new RangedRequest(typeof(int), typeof(int), 0, bounaryValue);

            // Exercise system
            var convertedValue = sut.GetConvertedMaximum(typeof(int));

            // Verify outcome
            Assert.Equal(42, convertedValue);
            // Teardown
        }

        [Fact]
        public void FailsWithMeaningfulExceptionWhenMinimumCannotBeConvertedWithoutOverflow()
        {
            // Fixture setup
            double valueWithOverflow = (double)long.MaxValue;

            var sut = new RangedRequest(typeof(long), typeof(double), valueWithOverflow, double.MaxValue);

            // Exercise system and verify outcome
            var actualEx = Assert.Throws<OverflowException>(() =>
                sut.GetConvertedMinimum(typeof(long)));
            Assert.Contains("To solve the issue", actualEx.Message);
            // Teardown
        }

        [Fact]
        public void FailsWithMeaningfulExceptionWhenMaximumCannotBeConvertedWithoutOverflow()
        {
            // Fixture setup
            double valueWithOverflow = (double)long.MaxValue;

            var sut = new RangedRequest(typeof(long), typeof(double), 0, valueWithOverflow);

            // Exercise system and verify outcome
            var actualEx = Assert.Throws<OverflowException>(() =>
                sut.GetConvertedMaximum(typeof(long)));
            Assert.Contains("To solve the issue", actualEx.Message);
            // Teardown
        }

        [Fact]
        public void ToStringShouldBeOverridden()
        {
            // Fixture setup
            var sut = new RangedRequest(typeof(long), typeof(int), 42, 100);

            // Exercise system
            var stringResult = sut.ToString();

            // Verify outcome
            Assert.Contains("Int32", stringResult);
            Assert.Contains("Int64", stringResult);
            Assert.Contains("42", stringResult);
            Assert.Contains("100", stringResult);

            // Teardown
        }
    }
}