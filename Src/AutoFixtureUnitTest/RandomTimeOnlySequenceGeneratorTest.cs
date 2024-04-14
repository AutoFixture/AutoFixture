#if NET6_0_OR_GREATER

using System;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest;

public class RandomTimeOnlySequenceGeneratorTest
{
    [Fact]
    public void SutIsSpecimenBuilder()
    {
        // Arrange & Act
        var sut = new RandomTimeOnlySequenceGenerator();

        // Assert
        Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
    }

    [Fact]
    public void InitializeWithInvertedRangeThrowsArgumentException()
    {
        // Arrange
        var minimum = new TimeOnly(12, 03, 21);
        var maximum = minimum.AddHours(3);

        // Act & assert
        Assert.Throws<ArgumentException>(
            () => new RandomTimeOnlySequenceGenerator(maximum, minimum));
    }

    [Fact]
    public void InitializeWithEmptyRangeThrowsArgumentException()
    {
        // Arrange
        var time = new TimeOnly(14, 13, 09);

        // Act & assert
        Assert.Throws<ArgumentException>(
            () => new RandomTimeOnlySequenceGenerator(time, time));
    }

    [Fact]
    public void CreateWithNullRequestReturnsNoSpecimen()
    {
        // Arrange
        var sut = new RandomTimeOnlySequenceGenerator();
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
        var sut = new RandomTimeOnlySequenceGenerator();

        // Act & assert
        Assert.Throws<ArgumentNullException>(
            () => sut.Create(typeof(TimeOnly), null));
    }

    [Theory]
    [InlineData("")]
    [InlineData(default(int))]
    [InlineData(default(bool))]
    public void CreateWithNonTypeRequestReturnsNoSpecimen(object request)
    {
        // Arrange
        var sut = new RandomTimeOnlySequenceGenerator();
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
    public void CreateWithNonTimeOnlyTypeRequestReturnsNoSpecimen(Type request)
    {
        // Arrange
        var sut = new RandomTimeOnlySequenceGenerator();
        var dummyContainer = new DelegatingSpecimenContext();

        // Act
        var result = sut.Create(request, dummyContainer);

        // Assert
        Assert.Equal(new NoSpecimen(), result);
    }

    [Fact]
    public void CreateWithTimeOnlyRequestReturnsTimeOnlyValue()
    {
        // Arrange
        var sut = new RandomTimeOnlySequenceGenerator();
        var dummyContainer = new DelegatingSpecimenContext();

        // Act
        var result = sut.Create(typeof(TimeOnly), dummyContainer);

        // Assert
        Assert.IsAssignableFrom<TimeOnly>(result);
    }

    [Fact]
    public void CreateWithTimeOnlyRequestReturnsADateWithinARangeOfPlusMinusTwoYearsFromNoon()
    {
        // Arrange
        var current = new TimeOnly(12, 00);
        var twoHoursAgo = current.AddHours(-6);
        var twoHoursForward = current.AddHours(6);
        var sut = new RandomTimeOnlySequenceGenerator();
        // Act
        var dummyContainer = new DelegatingSpecimenContext();
        var result = (TimeOnly)sut.Create(typeof(TimeOnly), dummyContainer);
        // Assert
        Assert.InRange(result, twoHoursAgo, twoHoursForward);
    }

    [Fact]
    public void CreateWithMultipleRequestsReturnsDifferentTimes()
    {
        // Arrange
        const int requestCount = 10;
        var times = Enumerable.Range(1, requestCount);
        var sut = new RandomTimeOnlySequenceGenerator();
        var dummyContainer = new DelegatingSpecimenContext();

        // Act
        var results = times
            .Select(t => sut.Create(typeof(TimeOnly), dummyContainer))
            .Cast<TimeOnly>();

        // Assert
        Assert.Equal(requestCount, results.Distinct().Count());
    }

    [Fact]
    public void CreateWithTimeOnlyRequestAndTimeRangeReturnsValueWithinThatRange()
    {
        // Arrange
        var minimum = new TimeOnly(13, 00);
        var maximum = minimum.AddHours(3);
        var sut = new RandomTimeOnlySequenceGenerator(minimum, maximum);
        // Act
        var dummyContainer = new DelegatingSpecimenContext();
        var result = (TimeOnly)sut.Create(typeof(TimeOnly), dummyContainer);
        // Assert
        Assert.InRange(result, minimum, maximum);
    }
}
#endif