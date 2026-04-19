using System;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest;

public class DecimalSequenceGeneratorTest
{
    [Fact]
    public void SutIsSpecimenBuilder()
    {
        // Arrange
        // Act
        var sut = new DecimalSequenceGenerator();
        // Assert
        Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
    }

    [Fact]
    public void CreateWithNullRequestWillReturnCorrectResult()
    {
        // Arrange
        var sut = new DecimalSequenceGenerator();
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
        var sut = new DecimalSequenceGenerator();
        // Act
        var dummyRequest = new object();
        sut.Create(dummyRequest, null);
        // Assert (no exception indicates success)
    }

    [Fact]
    public void CreateWithNonDecimalRequestWillReturnCorrectResult()
    {
        // Arrange
        var nonDecimalRequest = new object();
        var sut = new DecimalSequenceGenerator();
        // Act
        var dummyContainer = new DelegatingSpecimenContext();
        var result = sut.Create(nonDecimalRequest, dummyContainer);
        // Assert
        var expectedResult = NoSpecimen.Instance;
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void CreateWithDecimalRequestWillReturnCorrectResult()
    {
        // Arrange
        var decimalRequest = typeof(decimal);
        var sut = new DecimalSequenceGenerator();
        // Act
        var dummyContainer = new DelegatingSpecimenContext();
        var result = sut.Create(decimalRequest, dummyContainer);
        // Assert
        Assert.Equal(1m, result);
    }

    [Fact]
    public void CreateDecimalRequestWillReturnCorrectResultOnSecondCall()
    {
        // Arrange
        var decimalRequest = typeof(decimal);
        var dummyContainer = new DelegatingSpecimenContext();
        var loopTest = new LoopTest<DecimalSequenceGenerator, decimal>(sut => (decimal)sut.Create(decimalRequest, dummyContainer));
        // Act & assert
        loopTest.Execute(2);
    }

    [Fact]
    public void CreateWithDecimalRequestWillReturnCorrectResultOnTenthCall()
    {
        // Arrange
        var decimalRequest = typeof(decimal);
        var dummyContainer = new DelegatingSpecimenContext();
        var loopTest = new LoopTest<DecimalSequenceGenerator, decimal>(sut => (decimal)sut.Create(decimalRequest, dummyContainer));
        // Act & assert
        loopTest.Execute(10);
    }
}