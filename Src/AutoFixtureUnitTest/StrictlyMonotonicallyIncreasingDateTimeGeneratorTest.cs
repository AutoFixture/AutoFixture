using System;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest
{
    public class StrictlyMonotonicallyIncreasingDateTimeGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new StrictlyMonotonicallyIncreasingDateTimeGenerator();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestReturnsNoSpecimen()
        {
            // Fixture setup
            var sut = new StrictlyMonotonicallyIncreasingDateTimeGenerator();
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
            var sut = new NumericSequenceGenerator();
            // Exercise system and verify outcome
            var dummyRequest = new object();
            Assert.DoesNotThrow(() => sut.Create(dummyRequest, null));
            // Teardown
        }

        [Theory]
        [InlineData("")]
        [InlineData(default(int))]
        [InlineData(default(bool))]
        public void CreateWithNonTypeRequestReturnsNoSpecimen(object request)
        {
            // Fixture setup
            var sut = new StrictlyMonotonicallyIncreasingDateTimeGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen(request);
            Assert.Equal(expectedResult, result);
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
            var sut = new StrictlyMonotonicallyIncreasingDateTimeGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen(request);
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWithDateTimeRequestReturnsDateTimeValue()
        {
            // Fixture setup
            var dateTimeRequest = typeof(DateTime);
            var sut = new StrictlyMonotonicallyIncreasingDateTimeGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(dateTimeRequest, dummyContainer);
            // Verify outcome
            Assert.IsAssignableFrom<DateTime>(result);
            // Teardown
        }

        [Fact]
        public void CreateWithDateTimeRequestReturnsDifferentDay()
        {
            // Fixture setup
            var today = DateTime.Today;
            var dateTimeRequest = typeof(DateTime);
            var sut = new StrictlyMonotonicallyIncreasingDateTimeGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = (DateTime)sut.Create(dateTimeRequest, dummyContainer);
            // Verify outcome
            Assert.NotEqual(today, result.Date);
            // Teardown
        }

        [Fact]
        public void CreateWithMultipleDateTimeRequestsReturnsSequenceOfDates()
        {
            // Fixture setup
            var sequence = Enumerable.Range(1, 7);
            var expectedDates = sequence.Select(i => DateTime.Today.AddDays(i));
            var dateTimeRequest = typeof(DateTime);
            var sut = new StrictlyMonotonicallyIncreasingDateTimeGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var results = sequence.Select(i => (DateTime)sut.Create(dateTimeRequest, dummyContainer)).ToArray();
            // Verify outcome
            Assert.True(expectedDates.SequenceEqual(results.Select(i => i.Date)));
            // Teardown
        }
    }
}