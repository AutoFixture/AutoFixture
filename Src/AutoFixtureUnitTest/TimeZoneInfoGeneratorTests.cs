using System;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class TimeZoneInfoGeneratorTests
    {
        [Fact]
        public void WhenNullContext_ThrowsArgumentNullException()
        {
            // Arrange
            var sut = new TimeZoneInfoGenerator();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => sut.Create(typeof(TimeZoneInfo), null));
        }

        [Fact]
        public void WhenNullRequest_ReturnsNoSpecimen()
        {
            // Arrange
            var sut = new TimeZoneInfoGenerator();
            var context = new DelegatingSpecimenContext();

            // Act
            var result = sut.Create(null, context);

            // Assert
            Assert.IsType<NoSpecimen>(result);
        }

        [Fact]
        public void WhenNonTimeZoneInfoRequest_ReturnsNoSpecimen()
        {
            // Arrange
            var sut = new TimeZoneInfoGenerator();
            var context = new DelegatingSpecimenContext();

            // Act
            var result = sut.Create(typeof(string), context);

            // Assert
            Assert.IsType<NoSpecimen>(result);
        }

        [Fact]
        public void WhenTimeOffsetIsNoSpecimen_ReturnsNoSpecimen()
        {
            // Arrange
            var sut = new TimeZoneInfoGenerator();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = _ => NoSpecimen.Instance
            };

            // Act
            var result = sut.Create(typeof(TimeZoneInfo), context);

            // Assert
            Assert.IsType<NoSpecimen>(result);
        }

        [Fact]
        public void WhenTimeOffsetIsOmitSpecimen_ReturnsOmitSpecimen()
        {
            // Arrange
            var sut = new TimeZoneInfoGenerator();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = _ => new OmitSpecimen()
            };

            // Act
            var result = sut.Create(typeof(TimeZoneInfo), context);

            // Assert
            Assert.IsType<OmitSpecimen>(result);
        }

        [Fact]
        public void WhenTimeOffsetIsNotInt_ThrowsArgumentException()
        {
            // Arrange
            var sut = new TimeZoneInfoGenerator();
            var context = new DelegatingSpecimenContext { OnResolve = _ => "not int" };

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(
                () => sut.Create(typeof(TimeZoneInfo), context));
            Assert.Equal(
                "The result of ranged number request must be an int, but was System.String.",
                exception.Message);
        }

        [Fact]
        public void WhenTimeZoneInfoRequest_ReturnsCustomTimeZoneInfo()
        {
            // Arrange
            var sut = new TimeZoneInfoGenerator();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => r switch
                {
                    RangedNumberRequest _ => 2,
                    _ => NoSpecimen.Instance
                }
            };

            // Act
            var result = sut.Create(typeof(TimeZoneInfo), context);

            // Assert
            var tz = Assert.IsType<TimeZoneInfo>(result);
            Assert.Equal(TimeSpan.FromHours(2), tz.BaseUtcOffset);
            Assert.Equal("UTC+02", tz.Id);
            Assert.Equal("UTC+02", tz.StandardName);
            Assert.Equal("(UTC+02:00) Test Time Zone+02", tz.DisplayName);
        }
    }
}