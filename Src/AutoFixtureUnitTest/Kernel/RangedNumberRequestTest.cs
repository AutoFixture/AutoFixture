using System;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class RangedNumberRequestTest
    {
        [Fact]
        public void OperandTypeIsCorrect()
        {
            // Fixture setup
            var expectedOperandType = typeof(int);
            var sut = new RangedNumberRequest(expectedOperandType, 1, 3);
            // Exercise system
            var result = sut.OperandType;
            // Verify outcome
            Assert.Equal(expectedOperandType, result);
            // Teardown
        }

        [Fact]
        public void MinimumIsCorrect()
        {
            // Fixture setup
            var expectedMinimum = 1;
            var sut = new RangedNumberRequest(typeof(int), expectedMinimum, 3);
            // Exercise system
            var result = sut.Minimum;
            // Verify outcome
            Assert.Equal(expectedMinimum, result);
            // Teardown
        }

        [Fact]
        public void MaximumIsCorrect()
        {
            // Fixture setup
            var expectedMaximum = 3;
            var sut = new RangedNumberRequest(typeof(int), 1, expectedMaximum);
            // Exercise system
            var result = sut.Maximum;
            // Verify outcome
            Assert.Equal(expectedMaximum, result);
            // Teardown
        }

        [Fact]
        public void CreateWithNullOperandTypeWillThrow()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new RangedNumberRequest(null, 1, 3));
            // Teardown
        }

        [Fact]
        public void CreateWithNullMinimumWillThrow()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new RangedNumberRequest(typeof(int), null, 3));
            // Teardown
        }

        [Fact]
        public void CreateWithNullMaximumWillThrow()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new RangedNumberRequest(typeof(int), 1, null));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(int), 20, 10)]
        [InlineData(typeof(int), 10, 10)]
        [InlineData(typeof(int), -1, -2)]
        [InlineData(typeof(decimal), 20, 10)]
        [InlineData(typeof(decimal), 10, 10)]
        [InlineData(typeof(decimal), -1, -2)]
        [InlineData(typeof(double), 20, 10)]
        [InlineData(typeof(double), 10, 10)]
        [InlineData(typeof(double), -1, -2)]
        [InlineData(typeof(long), 20, 10)]
        [InlineData(typeof(long), 10, 10)]
        [InlineData(typeof(long), -1, -2)]
        public void CreateWithEqualOrBiggerMinimumThanMaximumWillThrow(Type type, object minimum, object maximum)
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new RangedNumberRequest(type, minimum, maximum));
            // Teardown
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
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() =>
                new RangedNumberRequest(type, minimum, maximum)));
            // Teardown
        }

        [Fact]
        public void SutIsEquatable()
        {
            // Fixture setup
            // Exercise system
            var sut = new RangedNumberRequest(typeof(int), 1, 3);
            // Verify outcome
            Assert.IsAssignableFrom<IEquatable<RangedNumberRequest>>(sut);
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualNullObject()
        {
            // Fixture setup
            var sut = new RangedNumberRequest(typeof(int), 1, 3);
            object other = null;
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualNullSut()
        {
            // Fixture setup
            var sut = new RangedNumberRequest(typeof(int), 1, 3);
            RangedNumberRequest other = null;
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualAnonymousObject()
        {
            // Fixture setup
            var sut = new RangedNumberRequest(typeof(int), 1, 3);
            object anonymousObject = new ConcreteType();
            // Exercise system
            var result = sut.Equals(anonymousObject);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualOtherObjectWhenOperandTypesDiffer()
        {
            // Fixture setup
            var sut      = new RangedNumberRequest(typeof(int), 1, 3);
            object other = new RangedNumberRequest(typeof(double), 1, 3);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualOtherSutWhenOperandTypesDiffer()
        {
            // Fixture setup
            var sut   = new RangedNumberRequest(typeof(int), 1, 3);
            var other = new RangedNumberRequest(typeof(double), 1, 3);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualOtherObjectWhenMinimumsDiffer()
        {
            // Fixture setup
            var sut      = new RangedNumberRequest(typeof(int), 1, 3);
            object other = new RangedNumberRequest(typeof(int), 2, 3);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualOtherSutWhenMinimumsDiffer()
        {
            // Fixture setup
            var sut   = new RangedNumberRequest(typeof(int), 1, 3);
            var other = new RangedNumberRequest(typeof(int), 2, 3);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualOtherObjectWhenMaximumsDiffer()
        {
            // Fixture setup
            var sut      = new RangedNumberRequest(typeof(int), 1, 3);
            object other = new RangedNumberRequest(typeof(int), 1, 4);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualOtherSutWhenMaximumsDiffer()
        {
            // Fixture setup
            var sut   = new RangedNumberRequest(typeof(int), 1, 3);
            var other = new RangedNumberRequest(typeof(int), 1, 4);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutEqualsOtherObjectWhenConstructorParametersEquals()
        {
            // Fixture setup
            Type operandType = typeof(int);
            object minimum = 1;
            object maximum = 3;
            var sut = new RangedNumberRequest(operandType, minimum, maximum);
            object other = new RangedNumberRequest(operandType, minimum, maximum);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutEqualsOtherSutWhenConstructorParametersEquals()
        {
            // Fixture setup
            Type operandType = typeof(int);
            object minimum = 1;
            object maximum = 3;
            var sut = new RangedNumberRequest(operandType, minimum, maximum);
            var other = new RangedNumberRequest(operandType, minimum, maximum);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result, "Equals");
            // Teardown
        }

        [Fact]
        public void GetHashCodeWillReturnCorrectResult()
        {
            // Fixture setup
            Type operandType = typeof(int);
            object minimum = 1;
            object maximum = 3;
            var sut = new RangedNumberRequest(operandType, minimum, maximum);
            var expectedHashCode = operandType.GetHashCode() ^ minimum.GetHashCode() ^ maximum.GetHashCode();
            // Exercise system
            var result = sut.GetHashCode();
            // Verify outcome
            Assert.Equal(expectedHashCode, result);
            // Teardown
        }
    }
}