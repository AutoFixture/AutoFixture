#if NET6_0_OR_GREATER

using System;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest;

public class RandomDateOnlySequenceGeneratorTest
{
    [Fact]
    public void SutIsSpecimenBuilder()
    {
        // Arrange & Act
        var sut = new RandomDateOnlySequenceGenerator();

        // Assert
        Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
    }

    [Fact]
    public void InitializeWithInvertedDateRangeThrowsArgumentException()
    {
        // Arrange
        var minDate = new DateOnly(2013, 07, 21);
        var maxDate = minDate.AddDays(3);

        // Act & assert
        Assert.Throws<ArgumentException>(
            () => new RandomDateOnlySequenceGenerator(maxDate, minDate));
    }

    [Fact]
    public void InitializeWithEmptyDateRangeThrowsArgumentException()
    {
        // Arrange
        var date = new DateOnly(2017, 08, 12);

        // Act & assert
        Assert.Throws<ArgumentException>(
            () => new RandomDateOnlySequenceGenerator(date, date));
    }

    [Fact]
    public void CreateWithNullRequestReturnsNoSpecimen()
    {
        // Arrange
        var sut = new RandomDateOnlySequenceGenerator();
        var dummyContainer = new DelegatingSpecimenContext();

        // Act
        var result = sut.Create(null, dummyContainer);

        // Assert
        Assert.Equal(new NoSpecimen(), result);
    }

    [Fact]
    public void CreateWithNullContextThrowsArgumentNullException()
    {
        // Arrange
        var sut = new RandomDateOnlySequenceGenerator();

        // Act & assert
        Assert.Throws<ArgumentNullException>(
            () => sut.Create(typeof(DateOnly), null));
    }

    [Theory]
    [InlineData("")]
    [InlineData(default(int))]
    [InlineData(default(bool))]
    public void CreateWithNonTypeRequestReturnsNoSpecimen(object request)
    {
        // Arrange
        var sut = new RandomDateOnlySequenceGenerator();
        var dummyContainer = new DelegatingSpecimenContext();

        // Act
        var result = sut.Create(request, dummyContainer);

        // Assert
        Assert.Equal(new NoSpecimen(), result);
    }

    [Theory]
    [InlineData(typeof(string))]
    [InlineData(typeof(object))]
    [InlineData(typeof(int))]
    [InlineData(typeof(bool))]
    public void CreateWithNonDateOnlyTypeRequestReturnsNoSpecimen(Type request)
    {
        // Arrange
        var sut = new RandomDateOnlySequenceGenerator();
        var dummyContainer = new DelegatingSpecimenContext();

        // Act
        var result = sut.Create(request, dummyContainer);

        // Assert
        Assert.Equal(new NoSpecimen(), result);
    }

    [Fact]
    public void CreateRequestReturnsDateTimeValue()
    {
        // Arrange
        var sut = new RandomDateOnlySequenceGenerator();
        var dummyContainer = new DelegatingSpecimenContext();

        // Act
        var result = sut.Create(typeof(DateOnly), dummyContainer);

        // Assert
        Assert.IsAssignableFrom<DateOnly>(result);
    }

    [Fact]
    public void CreateReturnsDateWithinARangeOfPlusMinusTwoYearsFromToday()
    {
        // Arrange
        var today = DateOnly.FromDateTime(DateTime.Today);
        var twoYearsAgo = today.AddYears(-2);
        var twoYearsForward = today.AddYears(2);
        var sut = new RandomDateOnlySequenceGenerator();
        var dummyContainer = new DelegatingSpecimenContext();

        // Act
        var result = (DateOnly)sut.Create(typeof(DateOnly), dummyContainer);

        // Assert
        Assert.InRange(result, twoYearsAgo, twoYearsForward);
    }

    [Fact]
    public void CreateWithMultipleRequestsReturnsDifferentDates()
    {
        // Arrange
        const int requestCount = 10;
        var times = Enumerable.Range(1, requestCount);
        var sut = new RandomDateOnlySequenceGenerator();
        var dummyContainer = new DelegatingSpecimenContext();

        // Act
        var results = times
            .Select(t => sut.Create(typeof(DateOnly), dummyContainer))
            .Cast<DateOnly>();

        // Assert
        Assert.Equal(requestCount, results.Distinct().Count());
    }

    [Fact]
    public void CreateRequestAndDateRangeReturnsValueWithinThatRange()
    {
        // Arrange
        var today = DateOnly.FromDateTime(DateTime.Today);
        var minDate = today;
        var maxDate = minDate.AddDays(3);
        var sut = new RandomDateOnlySequenceGenerator(minDate, maxDate);
        var dummyContainer = new DelegatingSpecimenContext();

        // Act
        var result = (DateOnly)sut.Create(typeof(DateOnly), dummyContainer);

        // Assert
        Assert.InRange(result, minDate, maxDate);
    }
}

#endif