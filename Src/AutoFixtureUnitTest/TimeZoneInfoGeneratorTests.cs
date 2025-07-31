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
        public void Create_WithNullContext_ThrowsArgumentNullException()
        {
            var sut = new TimeZoneInfoGenerator();

            Assert.Throws<ArgumentNullException>(() => sut.Create(typeof(TimeZoneInfo), null));
        }

        [Fact]
        public void Create_WithNullRequest_ReturnsCorrectResult()
        {
            var sut = new DomainNameGenerator();
            var context = new DelegatingSpecimenContext();

            var result = sut.Create(null, context);

            Assert.Equal(new NoSpecimen(), result);
        }

        [Fact]
        public void Create_WithNonTimeZoneInfoRequest_ReturnsNoSpecimen()
        {
            var sut = new TimeZoneInfoGenerator();
            var context = new DelegatingSpecimenContext();

            var result = sut.Create(typeof(string), context);

            Assert.IsType<NoSpecimen>(result);
        }

        [Fact]
        public void Create_WithTimeZoneInfoRequest_ReturnsCustomTimeZoneInfo()
        {
            var sut = new TimeZoneInfoGenerator();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r =>
                {
                    if (typeof(string).Equals(r))
                    {
                        return "testString";
                    }
                    else if (r is RangedNumberRequest)
                    {
                        return 2;
                    }

                    return new NoSpecimen();
                }
            };

            var result = sut.Create(typeof(TimeZoneInfo), context);

            var tz = Assert.IsType<TimeZoneInfo>(result);
            Assert.Equal("testString", tz.Id);
            Assert.Equal(TimeSpan.FromHours(2), tz.BaseUtcOffset);
            Assert.Equal("testString", tz.DisplayName);
            Assert.Equal("testString", tz.StandardName);
        }
    }
}