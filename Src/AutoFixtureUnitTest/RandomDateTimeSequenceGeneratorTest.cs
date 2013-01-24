using System;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest
{
    public class RandomDateTimeSequenceGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new RandomDateTimeSequenceGenerator();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithInvertedDateRangeThrowsArgumentException()
        {
            // Fixture setup
            var minDate = DateTime.Now;
            var maxDate = minDate.AddDays(3);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(
                () => new RandomDateTimeSequenceGenerator(maxDate, minDate));
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestReturnsNoSpecimen()
        {
            // Fixture setup
            var sut = new RandomDateTimeSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(null, dummyContainer);
            // Verify outcome
            Assert.Equal(new NoSpecimen(), result);
            // Teardown
        }

        [Fact]
        public void CreateWithNullContextDoesNotThrow()
        {
            // Fixture setup
            var sut = new RandomDateTimeSequenceGenerator();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(
                () => sut.Create(typeof(DateTime), null));
            // Teardown
        }

        [Theory]
        [InlineData("")]
        [InlineData(default(int))]
        [InlineData(default(bool))]
        public void CreateWithNonTypeRequestReturnsNoSpecimen(object request)
        {
            // Fixture setup
            var sut = new RandomDateTimeSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContainer);
            // Verify outcome
            Assert.Equal(new NoSpecimen(request), result);
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
            var sut = new RandomDateTimeSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContainer);
            // Verify outcome
            Assert.Equal(new NoSpecimen(request), result);
            // Teardown
        }

        [Fact]
        public void CreateWithDateTimeRequestReturnsDateTimeValue()
        {
            // Fixture setup
            var sut = new RandomDateTimeSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(typeof(DateTime), dummyContainer);
            // Verify outcome
            Assert.IsAssignableFrom<DateTime>(result);
            // Teardown
        }

        [Fact]
        public void CreateWithDateTimeRequestDoesNotReturnTodaysDate()
        {
            // Fixture setup
            var sut = new RandomDateTimeSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = (DateTime)sut.Create(typeof(DateTime), dummyContainer);
            // Verify outcome
            Assert.NotEqual(DateTime.Today, result.Date);
            // Teardown
        }

        [Fact]
        public void CreateWithMultipleDateTimeRequestsReturnsDifferentDates()
        {
            // Fixture setup
            const int requestCount = 10;
            var times = Enumerable.Range(1, requestCount);
            var sut = new RandomDateTimeSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var results = times
                .Select(t => sut.Create(typeof(DateTime), dummyContainer))
                .Cast<DateTime>();
            // Verify outcome
            Assert.Equal(requestCount, results.Distinct().Count());
            // Teardown
        }

        [Fact]
        public void CreateWithDateTimeRequestAndDateRangeReturnsDateWithinThatRange()
        {
            // Fixture setup
            var minDate = DateTime.Now;
            var maxDate = minDate.AddDays(3);
            var sut = new RandomDateTimeSequenceGenerator(minDate, maxDate);
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = (DateTime)sut.Create(typeof(DateTime), dummyContainer);
            // Verify outcome
            Assert.InRange(result, minDate, maxDate);
            // Teardown
        }

        [Fact]
        public void CreateWithDateTimeRequestAndSingleDateRangeReturnsThatSameDate()
        {
            // Fixture setup
            var date = DateTime.Now;
            var sut = new RandomDateTimeSequenceGenerator(date, date);
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = (DateTime)sut.Create(typeof(DateTime), dummyContainer);
            // Verify outcome
            Assert.Equal(result, date);
            // Teardown
        }
    }
}
