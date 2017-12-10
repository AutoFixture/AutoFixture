using System;
using AutoFixture.DataAnnotations;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.DataAnnotations
{
    public class EnumRangedRequestRelayTest
    {
        [Fact]
        public void SutShouldBeASpecimenBuilder()
        {
            // Arrange
            var sut = new EnumRangedRequestRelay();

            // Act & Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void ShouldFailForNullContext()
        {
            // Arrange
            var sut = new EnumRangedRequestRelay();
            var request = new object();

            // Act & Assert
            Assert.ThrowsAny<ArgumentNullException>(() =>
                sut.Create(request, null));
        }

        [Fact]
        public void ShouldNotHandleRequestIfMemberTypeIsNotEnum()
        {
            // Arrange
            var sut = new EnumRangedRequestRelay();
            var request =
                new RangedRequest(memberType: typeof(object), operandType: typeof(EnumType), minimum: 0, maximum: 1);
            var context = new DelegatingSpecimenContext();

            // Act
            var result = sut.Create(request, context);

            // Assert
            Assert.Equal(new NoSpecimen(), result);
        }

        [Fact]
        public void ShouldHandleRequestOfEnumType()
        {
            // Arrange
            var sut = new EnumRangedRequestRelay();
            var request =
                new RangedRequest(memberType: typeof(EnumType), operandType: typeof(int), minimum: 1, maximum: 2);
            var result = (int)EnumType.Second;
            var context = new DelegatingSpecimenContext
            {
                OnResolve = _ => result
            };

            // Act
            var actualResult = sut.Create(request, context);

            // Assert
            Assert.Equal(EnumType.Second, actualResult);
        }

        [Theory]
        [InlineData(EnumType.First, 1)]
        [InlineData(EnumType.Second, 2)]
        [InlineData(EnumType.Third, 3)]
        public void ShouldCorrectlyConvertNumericValue(EnumType expectedResult, int numericValue)
        {
            // Arrange
            var sut = new EnumRangedRequestRelay();
            var request =
                new RangedRequest(memberType: typeof(EnumType), operandType: typeof(int), minimum: 1, maximum: 3);
            var context = new DelegatingSpecimenContext
            {
                OnResolve = _ => numericValue
            };

            // Act
            var result = sut.Create(request, context);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void ShouldReturnNoSpecimenIfUnableToSatisfyRangedRequest()
        {
            // Arrange
            var sut = new EnumRangedRequestRelay();
            var request =
                new RangedRequest(memberType: typeof(EnumType), operandType: typeof(int), minimum: 1, maximum: 2);
            var context = new DelegatingSpecimenContext
            {
                OnResolve = _ => new NoSpecimen()
            };

            // Act
            var result = sut.Create(request, context);

            // Assert
            Assert.Equal(new NoSpecimen(), result);
        }

        [Theory]
        [InlineData(typeof(ShortEnumType), typeof(short))]
        [InlineData(typeof(ByteEnumType), typeof(byte))]
        [InlineData(typeof(LongEnumType), typeof(long))]
        public void ShouldRespectUnderlyingEnumType(Type enumType, Type underlyingType)
        {
            // Arrange
            var sut = new EnumRangedRequestRelay();
            var request =
                new RangedRequest(memberType: enumType, operandType: typeof(int), minimum: 1, maximum: 2);

            RangedNumberRequest capturedNumericRequest = null;
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r =>
                {
                    capturedNumericRequest = (RangedNumberRequest)r;
                    return new NoSpecimen();
                }
            };

            // Act
            sut.Create(request, context);

            // Assert
            Assert.NotNull(capturedNumericRequest);
            Assert.Equal(underlyingType, capturedNumericRequest.OperandType);
            Assert.IsType(underlyingType, capturedNumericRequest.Minimum);
            Assert.IsType(underlyingType, capturedNumericRequest.Maximum);
        }

        [Fact]
        public void ShouldCorrectPassMinimumAndMaximum()
        {
            // Arrange
            var sut = new EnumRangedRequestRelay();
            int minimum = 5;
            int maximum = 10;

            var requst = new RangedRequest(typeof(EnumType), typeof(int), minimum, maximum);
            RangedNumberRequest capturedNumericRequest = null;
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r =>
                {
                    capturedNumericRequest = (RangedNumberRequest)r;
                    return new NoSpecimen();
                }
            };

            // Act
            sut.Create(requst, context);

            // Assert
            Assert.NotNull(capturedNumericRequest);
            Assert.Equal(minimum, capturedNumericRequest.Minimum);
            Assert.Equal(maximum, capturedNumericRequest.Maximum);
        }

        private enum ShortEnumType : short
        {
            First,
            Second
        }

        private enum ByteEnumType : byte
        {
            First,
            Second
        }

        private enum LongEnumType : long
        {
            First,
            Second
        }
    }
}