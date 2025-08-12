using System;
using System.Linq;
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
            var sut = new TimeZoneInfoGenerator();

            Assert.Throws<ArgumentNullException>(
                () => sut.Create(typeof(TimeZoneInfo), null));
        }

        [Fact]
        public void WhenNullRequest_ReturnsNoSpecimen()
        {
            var sut = new TimeZoneInfoGenerator();
            var context = new DelegatingSpecimenContext();

            var result = sut.Create(null, context);

            Assert.IsType<NoSpecimen>(result);
        }

        [Fact]
        public void WhenNonTimeZoneInfoRequest_ReturnsNoSpecimen()
        {
            var sut = new TimeZoneInfoGenerator();
            var context = new DelegatingSpecimenContext();

            var result = sut.Create(typeof(string), context);

            Assert.IsType<NoSpecimen>(result);
        }

        [Fact]
        public void WhenTimeZoneInfoRequest_ReturnsCustomTimeZoneInfo()
        {
            var sut = new TimeZoneInfoGenerator();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => r switch
                {
                    RangedNumberRequest _ => 2,
                    _ => NoSpecimen.Instance
                }
            };

            var result = sut.Create(typeof(TimeZoneInfo), context);

            var tz = Assert.IsType<TimeZoneInfo>(result);
            Assert.Equal(TimeSpan.FromHours(2), tz.BaseUtcOffset);
            Assert.Equal("UTC+02", tz.Id);
            Assert.Equal("UTC+02", tz.StandardName);
            Assert.Equal("(UTC+02:00) Test Time Zone+02", tz.DisplayName);
        }
    }
}