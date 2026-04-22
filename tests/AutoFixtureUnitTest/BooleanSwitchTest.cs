using System;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest;

public class BooleanSwitchTest
{
    [Fact]
    public void SutIsSpecimenBuilder()
    {
        // Arrange
        // Act
        var sut = new BooleanSwitch();
        // Assert
        Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
    }

    [Fact]
    public void CreateWithNullRequestWillReturnCorrectResult()
    {
        // Arrange
        var sut = new BooleanSwitch();
        // Act
        var dummyContainer = new DelegatingSpecimenContext();
        var result = sut.Create(null, dummyContainer);
        // Assert
        Assert.Equal(NoSpecimen.Instance, result);
    }

    [Fact]
    public void CreateWithNullContainerDoesNotThrow()
    {
        // Arrange
        var sut = new BooleanSwitch();
        // Act
        var dummyRequest = new object();
        sut.Create(dummyRequest, null);
        // Assert (no exception indicates success)
    }

    [Fact]
    public void CreateWithNonBooleanRequestWillReturnCorrectResult()
    {
        // Arrange
        var nonBooleanRequest = new object();
        var sut = new BooleanSwitch();
        // Act
        var dummyContainer = new DelegatingSpecimenContext();
        var result = sut.Create(nonBooleanRequest, dummyContainer);
        // Assert
        var expectedResult = NoSpecimen.Instance;
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void CreateWithBooleanRequestWillReturnCorrectResultOnFirstCall()
    {
        // Arrange
        var booleanRequest = typeof(bool);
        var sut = new BooleanSwitch();
        // Act
        var dummyContainer = new DelegatingSpecimenContext();
        var result = sut.Create(booleanRequest, dummyContainer);
        // Assert
        Assert.True((bool)result);
    }

    [Fact]
    public void CreateWithBooleanRequestWillReturnCorrectResultOnSecondCall()
    {
        // Arrange
        var booleanRequest = typeof(bool);
        var sut = new BooleanSwitch();
        // Act
        var dummyContainer = new DelegatingSpecimenContext();
        sut.Create(booleanRequest, dummyContainer);
        var result = sut.Create(booleanRequest, dummyContainer);
        // Assert
        Assert.False((bool)result);
    }

    [Fact]
    public void CreateWithBooleanRequestWillReturnCorrectResultOnThirdCall()
    {
        // Arrange
        var booleanRequest = typeof(bool);
        var sut = new BooleanSwitch();
        // Act
        var dummyContainer = new DelegatingSpecimenContext();
        sut.Create(booleanRequest, dummyContainer);
        sut.Create(booleanRequest, dummyContainer);
        var result = sut.Create(booleanRequest, dummyContainer);
        // Assert
        Assert.True((bool)result);
    }

    [Fact]
    public void CreateWithBooleanRequestWillReturnCorrectResultOnFourthCall()
    {
        // Arrange
        var booleanRequest = typeof(bool);
        var sut = new BooleanSwitch();
        // Act
        var dummyContainer = new DelegatingSpecimenContext();
        sut.Create(booleanRequest, dummyContainer);
        sut.Create(booleanRequest, dummyContainer);
        sut.Create(booleanRequest, dummyContainer);
        var result = sut.Create(booleanRequest, dummyContainer);
        // Assert
        Assert.False((bool)result);
    }
}