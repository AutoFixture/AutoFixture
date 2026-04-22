using System;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest;

public class SingleSequenceGeneratorTest
{
    [Fact]
    public void SutIsSpecimenBuilder()
    {
        // Arrange
        // Act
        var sut = new SingleSequenceGenerator();
        // Assert
        Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
    }

    [Fact]
    public void CreateWithNullRequestWillReturnCorrectResult()
    {
        // Arrange
        var sut = new SingleSequenceGenerator();
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
        var sut = new SingleSequenceGenerator();
        // Act
        var dummyRequest = new object();
        sut.Create(dummyRequest, null);
        // Assert (no exception indicates success)
    }

    [Fact]
    public void CreateWithNonSingleRequestWillReturnCorrectResult()
    {
        // Arrange
        var nonSingleRequest = new object();
        var sut = new SingleSequenceGenerator();
        // Act
        var dummyContainer = new DelegatingSpecimenContext();
        var result = sut.Create(nonSingleRequest, dummyContainer);
        // Assert
        var expectedResult = NoSpecimen.Instance;
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void CreateWithSingleRequestWillReturnCorrectResult()
    {
        // Arrange
        var singleRequest = typeof(float);
        var sut = new SingleSequenceGenerator();
        // Act
        var dummyContainer = new DelegatingSpecimenContext();
        var result = sut.Create(singleRequest, dummyContainer);
        // Assert
        Assert.Equal(1f, result);
    }

    [Fact]
    public void CreateWithSingleRequestWillReturnCorrectResultOnSecondCall()
    {
        // Arrange
        var singleRequest = typeof(float);
        var dummyContainer = new DelegatingSpecimenContext();
        var loopTest = new LoopTest<SingleSequenceGenerator, float>(sut => (float)sut.Create(singleRequest, dummyContainer));
        // Act & assert
        loopTest.Execute(2);
    }

    [Fact]
    public void CreateWithSingleRequestWillReturnCorrectResultOnTenthCall()
    {
        // Arrange
        var singleRequest = typeof(float);
        var dummyContainer = new DelegatingSpecimenContext();
        var loopTest = new LoopTest<SingleSequenceGenerator, float>(sut => (float)sut.Create(singleRequest, dummyContainer));
        // Act & assert
        loopTest.Execute(10);
    }
}