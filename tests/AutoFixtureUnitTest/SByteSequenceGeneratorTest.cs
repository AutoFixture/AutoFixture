using System;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest;

public class SByteSequenceGeneratorTest
{
    [Fact]
    public void SutIsSpecimenBuilder()
    {
        // Arrange
        // Act
        var sut = new SByteSequenceGenerator();
        // Assert
        Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
    }

    [Fact]
    public void CreateWithNullRequestWillReturnCorrectResult()
    {
        // Arrange
        var sut = new SByteSequenceGenerator();
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
        var sut = new SByteSequenceGenerator();
        // Act
        var dummyRequest = new object();
        sut.Create(dummyRequest, null);
        // Assert (no exception indicates success)
    }

    [Fact]
    public void CreateWithNonSByteRequestWillReturnCorrectResult()
    {
        // Arrange
        var nonSByteRequest = new object();
        var sut = new SByteSequenceGenerator();
        // Act
        var dummyContainer = new DelegatingSpecimenContext();
        var result = sut.Create(nonSByteRequest, dummyContainer);
        // Assert
        var expectedResult = NoSpecimen.Instance;
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void CreateWithSByteRequestWillReturnCorrectResult()
    {
        // Arrange
        var sbyteRequest = typeof(sbyte);
        var sut = new SByteSequenceGenerator();
        // Act
        var dummyContainer = new DelegatingSpecimenContext();
        var result = sut.Create(sbyteRequest, dummyContainer);
        // Assert
        Assert.Equal((sbyte)1, result);
    }

    [Fact]
    public void CreateWithSByteRequestWillReturnCorrectResultOnSecondCall()
    {
        // Arrange
        var sbyteRequest = typeof(sbyte);
        var dummyContainer = new DelegatingSpecimenContext();
        var loopTest = new LoopTest<SByteSequenceGenerator, sbyte>(sut => (sbyte)sut.Create(sbyteRequest, dummyContainer));
        // Act & assert
        loopTest.Execute(2);
    }

    [Fact]
    public void CreateWithSByteRequestWillReturnCorrectResultOnTenthCall()
    {
        // Arrange
        var sbyteRequest = typeof(sbyte);
        var dummyContainer = new DelegatingSpecimenContext();
        var loopTest = new LoopTest<SByteSequenceGenerator, sbyte>(sut => (sbyte)sut.Create(sbyteRequest, dummyContainer));
        // Act & assert
        loopTest.Execute(10);
    }
}