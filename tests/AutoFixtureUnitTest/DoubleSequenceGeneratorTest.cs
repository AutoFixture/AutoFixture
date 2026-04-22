using System;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest;

public class DoubleSequenceGeneratorTest
{
    [Fact]
    public void SutIsSpecimenBuilder()
    {
        // Arrange
        // Act
        var sut = new DoubleSequenceGenerator();
        // Assert
        Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
    }

    [Fact]
    public void CreateWithNullRequestWillReturnCorrectResult()
    {
        // Arrange
        var sut = new DoubleSequenceGenerator();
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
        var sut = new DoubleSequenceGenerator();
        // Act
        var dummyRequest = new object();
        sut.Create(dummyRequest, null);
        // Assert (no exception indicates success)
    }

    [Fact]
    public void CreateWithNonDoubleRequestWillReturnCorrectResult()
    {
        // Arrange
        var nonDoubleRequest = new object();
        var sut = new DoubleSequenceGenerator();
        // Act
        var dummyContainer = new DelegatingSpecimenContext();
        var result = sut.Create(nonDoubleRequest, dummyContainer);
        // Assert
        var expectedResult = NoSpecimen.Instance;
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void CreateWithDoubleRequestWillReturnCorrectResult()
    {
        // Arrange
        var doubleRequest = typeof(double);
        var sut = new DoubleSequenceGenerator();
        // Act
        var dummyContainer = new DelegatingSpecimenContext();
        var result = sut.Create(doubleRequest, dummyContainer);
        // Assert
        Assert.Equal(1d, result);
    }

    [Fact]
    public void CreateWithDoubleRequestWillReturnCorrectResultOnSecondCall()
    {
        // Arrange
        var doubleRequest = typeof(double);
        var dummyContainer = new DelegatingSpecimenContext();
        var loopTest = new LoopTest<DoubleSequenceGenerator, double>(sut => (double)sut.Create(doubleRequest, dummyContainer));
        // Act & assert
        loopTest.Execute(2);
    }

    [Fact]
    public void CreateWithDoubleRequestWillReturnCorrectResultOnTenthCall()
    {
        // Arrange
        var doubleRequest = typeof(double);
        var dummyContainer = new DelegatingSpecimenContext();
        var loopTest = new LoopTest<DoubleSequenceGenerator, double>(sut => (double)sut.Create(doubleRequest, dummyContainer));
        // Act & assert
        loopTest.Execute(10);
    }
}