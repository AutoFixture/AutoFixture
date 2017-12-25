using System;
using System.Linq;
using System.Threading;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class StrictlyMonotonicallyIncreasingDateTimeGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            var seed = DateTime.Now;
            // Act
            var sut = new StrictlyMonotonicallyIncreasingDateTimeGenerator(seed);
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestReturnsNoSpecimen()
        {
            // Arrange
            var seed = DateTime.Now;
            var sut = new StrictlyMonotonicallyIncreasingDateTimeGenerator(seed);
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(null, dummyContainer);
            // Assert
            Assert.Equal(new NoSpecimen(), result);
        }

        [Theory]
        [InlineData("")]
        [InlineData(default(int))]
        [InlineData(default(bool))]
        public void CreateWithNonTypeRequestReturnsNoSpecimen(object request)
        {
            // Arrange
            var seed = DateTime.Now;
            var sut = new StrictlyMonotonicallyIncreasingDateTimeGenerator(seed);
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContainer);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(typeof(string))]
        [InlineData(typeof(object))]
        [InlineData(typeof(int))]
        [InlineData(typeof(bool))]
        public void CreateWithNonDateTimeTypeRequestReturnsNoSpecimen(Type request)
        {
            // Arrange
            var seed = DateTime.Now;
            var sut = new StrictlyMonotonicallyIncreasingDateTimeGenerator(seed);
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContainer);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWithDateTimeRequestReturnsDateTimeValue()
        {
            // Arrange
            var request = typeof(DateTime);
            var seed = DateTime.Now;
            var sut = new StrictlyMonotonicallyIncreasingDateTimeGenerator(seed);
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContainer);
            // Assert
            Assert.IsAssignableFrom<DateTime>(result);
        }

        [Fact]
        public void CreateWithDateTimeRequestReturnsDifferentDay()
        {
            // Arrange
            var request = typeof(DateTime);
            var seed = DateTime.Now;
            var sut = new StrictlyMonotonicallyIncreasingDateTimeGenerator(seed);
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = (DateTime)sut.Create(request, dummyContainer);
            // Assert
            Assert.NotEqual(DateTime.Today, result.Date);
        }

        [Fact]
        public void CreateWithMultipleDateTimeRequestsReturnsSequenceOfDates()
        {
            // Arrange
            var sequence = Enumerable.Range(1, 7);
            var expectedDates = sequence.Select(i => DateTime.Today.AddDays(i));
            var request = typeof(DateTime);
            var seed = DateTime.Now;
            var sut = new StrictlyMonotonicallyIncreasingDateTimeGenerator(seed);
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var results = sequence.Select(i => (DateTime)sut.Create(request, dummyContainer)).ToArray();
            // Assert
            Assert.True(expectedDates.SequenceEqual(results.Select(i => i.Date)));
        }

        [Fact]
        public void CreateWithDateTimeRequestsTwiceWithinMillisecondsReturnsDatesExactlyOneDayApart()
        {
            // Arrange
            var nowResolution = TimeSpan.FromMilliseconds(10); // see http://msdn.microsoft.com/en-us/library/system.datetime.now.aspx
            var request = typeof(DateTime);
            var seed = DateTime.Now;
            var sut = new StrictlyMonotonicallyIncreasingDateTimeGenerator(seed);
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var firstResult = (DateTime)sut.Create(request, dummyContainer);
            Thread.Sleep(nowResolution + nowResolution);
            var secondResult = (DateTime)sut.Create(request, dummyContainer);
            // Assert
            Assert.Equal(firstResult.AddDays(1), secondResult);
        }

        [Fact]
        public void CreateWithDateTimeRequestAndSeedValueReturnsSeedValuePlusOneDay()
        {
            // Arrange
            var request = typeof(DateTime);
            var seed = DateTime.Now.AddDays(3);
            var sut = new StrictlyMonotonicallyIncreasingDateTimeGenerator(seed);
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = (DateTime)sut.Create(request, dummyContainer);
            // Assert
            Assert.Equal(seed.AddDays(1), result);
        }
    }
}
