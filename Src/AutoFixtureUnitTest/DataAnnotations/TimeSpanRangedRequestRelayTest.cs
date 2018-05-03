using System;
using AutoFixture.DataAnnotations;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.DataAnnotations
{
    public class TimeSpanRangedRequestRelayTest
    {
        [Fact]
        public void SutShouldBeASpecimenBuilder()
        {
            // Arrange
            var sut = new TimeSpanRangedRequestRelay();

            // Act & Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }


        [Fact]
        public void ShouldFailForNullContext()
        {
            // Arrange
            var sut = new TimeSpanRangedRequestRelay();
            var request = new object();

            // Act & Assert
            Assert.ThrowsAny<ArgumentNullException>(() =>
                sut.Create(request, null));
        }

        [Fact]
        public void ShouldNotHandleRequestIfMemberTypeIsNotTimeSpan()
        {
            // Arrange
            var sut = new TimeSpanRangedRequestRelay();
            var request =
                new RangedRequest(memberType: typeof(object), operandType: typeof(object), minimum: 0, maximum: 1);
            var context = new DelegatingSpecimenContext();

            // Act
            var result = sut.Create(request, context);

            // Assert
            Assert.Equal(new NoSpecimen(), result);
        }

        [Fact]
        public void ShouldHandleRequestOfTimeSpanType()
        {
            // Arrange
            var sut = new TimeSpanRangedRequestRelay();
            var request =
                new RangedRequest(memberType: typeof(TimeSpan), operandType: typeof(TimeSpan), minimum: "00:00:00",
                    maximum: "01:00:00");

            var context = new DelegatingSpecimenContext
            {
                OnResolve = _ => 30 * 60 * 1000.0 //30 (chosen randomly) minutes in miliseconds
            };

            // Act
            var actualResult = sut.Create(request, context);

            // Assert
            Assert.Equal(TimeSpan.FromMinutes(30), actualResult);
        }

        [Fact]
        public void ShouldReturnNoSpecimenIfUnableToSatisfyRangedRequest()
        {
            // Arrange
            var sut = new TimeSpanRangedRequestRelay();
            var request =
                new RangedRequest(memberType: typeof(TimeSpan), operandType: typeof(TimeSpan), minimum: "00:00:00",
                    maximum: "01:00:00");

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
        public void ShouldCorrectPassMinimumAndMaximumAsMilliseconds()
        {
            // Arrange
            var sut = new TimeSpanRangedRequestRelay();

            var request = new RangedRequest(typeof(TimeSpan), typeof(TimeSpan), "00:00:00", "12:00:00");
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
            Assert.Equal(0.0, capturedNumericRequest.Minimum);
            Assert.Equal(12 * 60 * 60 * 1000.0, capturedNumericRequest.Maximum);
        }
    }
}