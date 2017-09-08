using System;
using System.Linq;
using System.Threading;
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
            var seed = DateTime.Now;
            // Exercise system
            var sut = new StrictlyMonotonicallyIncreasingDateTimeGenerator(seed);
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestReturnsNoSpecimen()
        {
            // Fixture setup
            var seed = DateTime.Now;
            var sut = new StrictlyMonotonicallyIncreasingDateTimeGenerator(seed);
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(null, dummyContainer);
            // Verify outcome
            Assert.Equal(new NoSpecimen(), result);
            // Teardown
        }

        [Theory]
        [InlineData("")]
        [InlineData(default(int))]
        [InlineData(default(bool))]
        public void CreateWithNonTypeRequestReturnsNoSpecimen(object request)
        {
            // Fixture setup
            var seed = DateTime.Now;
            var sut = new StrictlyMonotonicallyIncreasingDateTimeGenerator(seed);
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen();
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
            var seed = DateTime.Now;
            var sut = new StrictlyMonotonicallyIncreasingDateTimeGenerator(seed);
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWithDateTimeRequestReturnsDateTimeValue()
        {
            // Fixture setup
            var request = typeof(DateTime);
            var seed = DateTime.Now;
            var sut = new StrictlyMonotonicallyIncreasingDateTimeGenerator(seed);
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContainer);
            // Verify outcome
            Assert.IsAssignableFrom<DateTime>(result);
            // Teardown
        }

        [Fact]
        public void CreateWithDateTimeRequestReturnsDifferentDay()
        {
            // Fixture setup
            var request = typeof(DateTime);
            var seed = DateTime.Now;
            var sut = new StrictlyMonotonicallyIncreasingDateTimeGenerator(seed);
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = (DateTime)sut.Create(request, dummyContainer);
            // Verify outcome
            Assert.NotEqual(DateTime.Today, result.Date);
            // Teardown
        }

        [Fact]
        public void CreateWithMultipleDateTimeRequestsReturnsSequenceOfDates()
        {
            // Fixture setup
            var sequence = Enumerable.Range(1, 7);
            var expectedDates = sequence.Select(i => DateTime.Today.AddDays(i));
            var request = typeof(DateTime);
            var seed = DateTime.Now;
            var sut = new StrictlyMonotonicallyIncreasingDateTimeGenerator(seed);
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var results = sequence.Select(i => (DateTime)sut.Create(request, dummyContainer)).ToArray();
            // Verify outcome
            Assert.True(expectedDates.SequenceEqual(results.Select(i => i.Date)));
            // Teardown
        }

        [Fact]
        public void CreateWithDateTimeRequestsTwiceWithinMillisecondsReturnsDatesExactlyOneDayApart()
        {
            // Fixture setup
            var nowResolution = TimeSpan.FromMilliseconds(10); // see http://msdn.microsoft.com/en-us/library/system.datetime.now.aspx
            var request = typeof(DateTime);
            var seed = DateTime.Now;
            var sut = new StrictlyMonotonicallyIncreasingDateTimeGenerator(seed);
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var firstResult = (DateTime)sut.Create(request, dummyContainer);
            Thread.Sleep(nowResolution + nowResolution);
            var secondResult = (DateTime)sut.Create(request, dummyContainer);
            // Verify outcome
            Assert.Equal(firstResult.AddDays(1), secondResult);
            // Teardown
        }

        [Fact]
        public void CreateWithDateTimeRequestAndSeedValueReturnsSeedValuePlusOneDay()
        {
            // Fixture setup
            var request = typeof(DateTime);
            var seed = DateTime.Now.AddDays(3);
            var sut = new StrictlyMonotonicallyIncreasingDateTimeGenerator(seed);
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = (DateTime)sut.Create(request, dummyContainer);
            // Verify outcome
            Assert.Equal(seed.AddDays(1), result);
            // Teardown
        }
    }
}
