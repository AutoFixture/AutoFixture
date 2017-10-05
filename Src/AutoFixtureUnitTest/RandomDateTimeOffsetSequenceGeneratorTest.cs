using System;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest
{
    public class RandomDateTimeOffsetSequenceGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new RandomDateTimeOffsetSequenceGenerator();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithInvertedDateRangeThrowsArgumentException()
        {
            // Fixture setup
            var minDate = DateTimeOffset.Now;
            var maxDate = minDate.AddDays(3);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(
                () => new RandomDateTimeOffsetSequenceGenerator(maxDate, minDate));
            // Teardown
        }

        [Fact]
        public void InitializeWithEmptyDateRangeThrowsArgumentException()
        {
            // Fixture setup
            var date = DateTimeOffset.Now;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(
                () => new RandomDateTimeOffsetSequenceGenerator(date, date));
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestReturnsNoSpecimen()
        {
            // Fixture setup
            var sut = new RandomDateTimeOffsetSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(null, dummyContainer);
            // Verify outcome
            Assert.Equal(new NoSpecimen(), result);
            // Teardown
        }

        [Fact]
        public void CreateWithNullContextThrowsArgumentNullException()
        {
            // Fixture setup
            var sut = new RandomDateTimeOffsetSequenceGenerator();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(
                () => sut.Create(typeof(DateTimeOffset), null));
            // Teardown
        }

        [Theory]
        [InlineData("")]
        [InlineData(default(int))]
        [InlineData(default(bool))]
        public void CreateWithNonTypeRequestReturnsNoSpecimen(object request)
        {
            // Fixture setup
            var sut = new RandomDateTimeOffsetSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContainer);
            // Verify outcome
            Assert.Equal(new NoSpecimen(), result);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(string))]
        [InlineData(typeof(object))]
        [InlineData(typeof(int))]
        [InlineData(typeof(bool))]
        public void CreateWithNonDateTimeTypeRequestReturnsNoSpecimen(Type request)
        {
            // Fixture setup
            var sut = new RandomDateTimeOffsetSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContainer);
            // Verify outcome
            Assert.Equal(new NoSpecimen(), result);
            // Teardown
        }

        [Fact]
        public void CreateWithDateTimeOffsetRequestReturnsDateTimeOffsetValue()
        {
            // Fixture setup
            var sut = new RandomDateTimeOffsetSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(typeof(DateTimeOffset), dummyContainer);
            // Verify outcome
            Assert.IsAssignableFrom<DateTimeOffset>(result);
            // Teardown
        }

        [Fact]
        public void CreateWithDateTimeOffsetRequestReturnsADateWithinARangeOfPlusMinusTwoYearsFromToday()
        {
            // Fixture setup
            var twoYearsAgo = DateTimeOffset.UtcNow.AddYears(-2);
            var twoYearsForward = DateTimeOffset.UtcNow.AddYears(2);
            var sut = new RandomDateTimeOffsetSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = (DateTimeOffset)sut.Create(typeof(DateTimeOffset), dummyContainer);
            // Verify outcome
            Assert.InRange(result, twoYearsAgo, twoYearsForward);
            // Teardown
        }

        [Fact]
        public void CreateWithMultipleDateTimeOffsetRequestsReturnsDifferentDates()
        {
            // Fixture setup
            const int requestCount = 10;
            var times = Enumerable.Range(1, requestCount);
            var sut = new RandomDateTimeOffsetSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var results = times
                .Select(t => sut.Create(typeof(DateTimeOffset), dummyContainer))
                .Cast<DateTimeOffset>();
            // Verify outcome
            Assert.Equal(requestCount, results.Distinct().Count());
            // Teardown
        }

        [Fact]
        public void CreateWithDateTimeOffsetRequestAndDateRangeReturnsDateWithinThatRange()
        {
            // Fixture setup
            var minDate = DateTimeOffset.UtcNow;
            var maxDate = minDate.AddDays(3);
            var sut = new RandomDateTimeOffsetSequenceGenerator(minDate, maxDate);
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = (DateTimeOffset)sut.Create(typeof(DateTimeOffset), dummyContainer);
            // Verify outcome
            Assert.InRange(result.ToUniversalTime(), minDate, maxDate);
            // Teardown
        }
    }
}