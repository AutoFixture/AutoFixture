using System;
using AutoFixture.DataAnnotations;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.DataAnnotations
{
    public class NumericRangedRequestRelayTest
    {
        [Fact]
        public void SutShouldBeASpecimenBuilder()
        {
            // Arrange
            var sut = new NumericRangedRequestRelay();

            // Act & assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void ShouldFailWithArgumentNullExceptionForNullContext()
        {
            // Arrange
            var sut = new NumericRangedRequestRelay();
            var request = new object();
            ISpecimenContext nullContext = null;

            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(request, nullContext));
        }

        [Fact]
        public void ShouldReturnNoResultForNullRequest()
        {
            // Arrange
            var sut = new NumericRangedRequestRelay();
            object nullRequest = null;
            var dummyContext = new DelegatingSpecimenContext();

            // Act
            var result = sut.Create(nullRequest, dummyContext);

            // Assert
            Assert.IsType<NoSpecimen>(result);
        }

        public static TheoryData<object> NonSupportedRequests => new TheoryData<object>
        {
            new object(),
            typeof(object),
            typeof(int),
            typeof(string),
            new RangedNumberRequest(typeof(int), 0, 42)
        };

        [Theory, MemberData(nameof(NonSupportedRequests))]
        public void ShouldReturnNoResultForNonSupportedRequests(object request)
        {
            // Arrange
            var sut = new NumericRangedRequestRelay();
            var context = new DelegatingSpecimenContext();

            // Act
            var result = sut.Create(request, context);

            // Assert
            Assert.IsType<NoSpecimen>(result);
        }

        [Theory]
        [InlineData(typeof(string))]
        [InlineData(typeof(object))]
        [InlineData(typeof(ConcreteType))]
        [InlineData(typeof(StringComparison))]
        public void ShouldReturnNoResultIfRangedRequestIsOfNonNumericMemberType(Type memberType)
        {
            // Arrange
            var sut = new NumericRangedRequestRelay();
            var request = new RangedRequest(memberType, typeof(int), 0, 42);
            var context = new DelegatingSpecimenContext();

            // Act
            var result = sut.Create(request, context);

            // Assert
            Assert.IsType<NoSpecimen>(result);
        }

        [Theory]
        [InlineData(typeof(byte))]
        [InlineData(typeof(sbyte))]
        [InlineData(typeof(short))]
        [InlineData(typeof(ushort))]
        [InlineData(typeof(int))]
        [InlineData(typeof(uint))]
        [InlineData(typeof(long))]
        [InlineData(typeof(ulong))]
        [InlineData(typeof(float))]
        [InlineData(typeof(double))]
        [InlineData(typeof(decimal))]
        public void ShouldRelayForRangedRequestsOfNumericMemberType(Type requestType)
        {
            // Arrange
            var sut = new NumericRangedRequestRelay();

            var request = new RangedRequest(requestType, requestType, 0, 42);
            var expectedResult = new object();
            var context = new DelegatingSpecimenContext
            {
                OnResolve =
                    r => r is RangedNumberRequest rr && rr.OperandType == requestType
                        ? expectedResult
                        : new NoSpecimen()
            };

            // Act
            var result = sut.Create(request, context);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(typeof(long), 10L, 42L)]
        [InlineData(typeof(ushort), (ushort)10, (ushort)42)]
        [InlineData(typeof(double), 10.0, 42.0)]
        [InlineData(typeof(float), 10.0F, 42.0F)]
        public void ShouldConvertBoundaryToMemberType(Type memberType, object castedMin, object castedMax)
        {
            // Arrange
            var sut = new NumericRangedRequestRelay();

            var request = new RangedRequest(memberType, typeof(int), 10, 42);
            var expectedResult = new object();
            var expectedNestedRequest = new RangedNumberRequest(memberType, castedMin, castedMax);
            var context = new DelegatingSpecimenContext
            {
                OnResolve =
                    r => r.Equals(expectedNestedRequest)
                        ? expectedResult
                        : new NoSpecimen()
            };

            // Act
            var result = sut.Create(request, context);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void ShouldFailWithMeaningfulExceptionIfUnableToCastWithoutOverflow()
        {
            // Arrange
            var sut = new NumericRangedRequestRelay();

            double overflowedValue = long.MaxValue;
            var request = new RangedRequest(typeof(long), typeof(double), 0, overflowedValue);
            var context = new DelegatingSpecimenContext();

            // Act & assert
            var ex = Assert.Throws<OverflowException>(() =>
                sut.Create(request, context));
            Assert.Contains("To solve the issue", ex.Message);
        }
    }
}