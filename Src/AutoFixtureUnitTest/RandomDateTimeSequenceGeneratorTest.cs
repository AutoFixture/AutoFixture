using System;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class RandomDateTimeSequenceGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new RandomDateTimeSequenceGenerator();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void InitializeWithInvertedDateRangeThrowsArgumentException()
        {
            // Arrange
            var minDate = DateTime.Now;
            var maxDate = minDate.AddDays(3);
            // Act & assert
            Assert.Throws<ArgumentException>(
                () => new RandomDateTimeSequenceGenerator(maxDate, minDate));
        }

        [Fact]
        public void InitializeWithEmptyDateRangeThrowsArgumentException()
        {
            // Arrange
            var date = DateTime.Now;
            // Act & assert
            Assert.Throws<ArgumentException>(
                () => new RandomDateTimeSequenceGenerator(date, date));
        }

        [Fact]
        public void CreateWithNullRequestReturnsNoSpecimen()
        {
            // Arrange
            var sut = new RandomDateTimeSequenceGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(null, dummyContainer);
            // Assert
            Assert.Equal(new NoSpecimen(), result);
        }

        [Fact]
        public void CreateWithNullContextThrowsArgumentNullException()
        {
            // Arrange
            var sut = new RandomDateTimeSequenceGenerator();
            // Act & assert
            Assert.Throws<ArgumentNullException>(
                () => sut.Create(typeof(DateTime), null));
        }

        [Theory]
        [InlineData("")]
        [InlineData(default(int))]
        [InlineData(default(bool))]
        public void CreateWithNonTypeRequestReturnsNoSpecimen(object request)
        {
            // Arrange
            var sut = new RandomDateTimeSequenceGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContainer);
            // Assert
            Assert.Equal(new NoSpecimen(), result);
        }

        [Theory]
        [InlineData(typeof(string))]
        [InlineData(typeof(object))]
        [InlineData(typeof(int))]
        [InlineData(typeof(bool))]
        public void CreateWithNonDateTimeTypeRequestReturnsNoSpecimen(Type request)
        {
            // Arrange
            var sut = new RandomDateTimeSequenceGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContainer);
            // Assert
            Assert.Equal(new NoSpecimen(), result);
        }

        [Fact]
        public void CreateWithDateTimeRequestReturnsDateTimeValue()
        {
            // Arrange
            var sut = new RandomDateTimeSequenceGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(typeof(DateTime), dummyContainer);
            // Assert
            Assert.IsAssignableFrom<DateTime>(result);
        }

        [Fact]
        public void CreateWithDateTimeRequestReturnsADateWithinARangeOfPlusMinusTwoYearsFromToday()
        {
            // Arrange
            var twoYearsAgo = DateTime.Today.AddYears(-2);
            var twoYearsForward = DateTime.Today.AddYears(2);
            var sut = new RandomDateTimeSequenceGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = (DateTime)sut.Create(typeof(DateTime), dummyContainer);
            // Assert
            Assert.InRange(result, twoYearsAgo, twoYearsForward);
        }

        [Fact]
        public void CreateWithMultipleDateTimeRequestsReturnsDifferentDates()
        {
            // Arrange
            const int requestCount = 10;
            var times = Enumerable.Range(1, requestCount);
            var sut = new RandomDateTimeSequenceGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var results = times
                .Select(t => sut.Create(typeof(DateTime), dummyContainer))
                .Cast<DateTime>();
            // Assert
            Assert.Equal(requestCount, results.Distinct().Count());
        }

        [Fact]
        public void CreateWithDateTimeRequestAndDateRangeReturnsDateWithinThatRange()
        {
            // Arrange
            var minDate = DateTime.Now;
            var maxDate = minDate.AddDays(3);
            var sut = new RandomDateTimeSequenceGenerator(minDate, maxDate);
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = (DateTime)sut.Create(typeof(DateTime), dummyContainer);
            // Assert
            Assert.InRange(result, minDate, maxDate);
        }
    }
}
