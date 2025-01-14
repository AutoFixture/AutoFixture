using System;
using AutoFixture.DataAnnotations;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.DataAnnotations
{
    public class DateTimeRangedRequestRelayTest
    {
        [Fact]
        public void SutShouldBeASpecimenBuilder()
        {
            // Arrange
            var sut = new DateTimeRangedRequestRelay();

            // Act & Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void ShouldFailForNullContext()
        {
            // Arrange
            var sut = new DateTimeRangedRequestRelay();
            var request = new object();

            // Act & Assert
            Assert.ThrowsAny<ArgumentNullException>(() =>
                sut.Create(request, null));
        }

        [Fact]
        public void ShouldNotHandleRequestIfMemberTypeIsNotDateTime()
        {
            // Arrange
            var sut = new DateTimeRangedRequestRelay();
            var request =
                new RangedRequest(memberType: typeof(object), operandType: typeof(object), minimum: 0, maximum: 1);
            var context = new DelegatingSpecimenContext();

            // Act
            var result = sut.Create(request, context);

            // Assert
            Assert.Equal(new NoSpecimen(), result);
        }

        [Fact]
        public void ShouldHandleRequestOfDateTimeType()
        {
            // Arrange
            var sut = new DateTimeRangedRequestRelay();
            var request =
                new RangedRequest(memberType: typeof(DateTime), operandType: typeof(DateTime), minimum: "2020-01-01 00:00:00",
                    maximum: "2020-12-31 23:59:59");

            var context = new DelegatingSpecimenContext
            {
                OnResolve = _ => new DateTime(2020, 06, 15, 12, 30, 30).Ticks
            };

            // Act
            var actualResult = sut.Create(request, context);

            // Assert
            Assert.Equal(new DateTime(2020, 06, 15, 12, 30, 30), actualResult);
        }

        [Fact]
        public void ShouldReturnNoSpecimenIfUnableToSatisfyRangedRequest()
        {
            // Arrange
            var sut = new DateTimeRangedRequestRelay();
            var request =
                new RangedRequest(memberType: typeof(DateTime), operandType: typeof(DateTime), minimum: "2020-01-01 00:00:00",
                    maximum: "2020-12-31 23:59:59");

            var context = new DelegatingSpecimenContext
            {
                OnResolve = _ => new NoSpecimen()
            };

            // Act
            var actualResult = sut.Create(request, context);

            // Assert
            Assert.Equal(new NoSpecimen(), actualResult);
        }

        [Fact]
        public void ShouldCorrectPassMinimumAndMaximumAsTicks()
        {
            // Arrange
            var sut = new DateTimeRangedRequestRelay();

            var request = new RangedRequest(typeof(DateTime), typeof(DateTime), "2020-01-01 00:00:00", "2020-12-31 23:59:59");
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
            Assert.Equal(new DateTime(2020, 1, 1, 0, 0, 0).Ticks, capturedNumericRequest.Minimum);
            Assert.Equal(new DateTime(2020, 12, 31, 23, 59, 59).Ticks, capturedNumericRequest.Maximum);
        }
    }
}
