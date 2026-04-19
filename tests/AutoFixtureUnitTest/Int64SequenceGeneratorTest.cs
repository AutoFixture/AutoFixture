using System;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest;

public class Int64SequenceGeneratorTest
{
    [Fact]
    public void SutIsSpecimenBuilder()
    {
        // Arrange
        // Act
        var sut = new Int64SequenceGenerator();
        // Assert
        Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
    }

    [Fact]
    public void CreateWithNullRequestWillReturnCorrectResult()
    {
        // Arrange
        var sut = new Int64SequenceGenerator();
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
        var sut = new Int64SequenceGenerator();
        // Act
        var dummyRequest = new object();
        sut.Create(dummyRequest, null);
        // Assert (no exception indicates success)
    }

    [Fact]
    public void CreateWithNonInt64RequestWillReturnCorrectResult()
    {
        // Arrange
        var nonInt64Request = new object();
        var sut = new Int64SequenceGenerator();
        // Act
        var dummyContainer = new DelegatingSpecimenContext();
        var result = sut.Create(nonInt64Request, dummyContainer);
        // Assert
        var expectedResult = NoSpecimen.Instance;
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void CreateWithInt64RequestWillReturnCorrectResult()
    {
        // Arrange
        var int64Request = typeof(long);
        var sut = new Int64SequenceGenerator();
        // Act
        var dummyContainer = new DelegatingSpecimenContext();
        var result = sut.Create(int64Request, dummyContainer);
        // Assert
        Assert.Equal(1L, result);
    }

    [Fact]
    public void CreateWithInt64RequestWillReturnCorrectResultOnSecondCall()
    {
        // Arrange
        var int64Request = typeof(long);
        var dummyContainer = new DelegatingSpecimenContext();
        var loopTest = new LoopTest<Int64SequenceGenerator, long>(sut => (long)sut.Create(int64Request, dummyContainer));
        // Act & assert
        loopTest.Execute(2);
    }

    [Fact]
    public void CreateWithInt64RequestWillReturnCorrectResultOnTenthCall()
    {
        // Arrange
        var int64Request = typeof(long);
        var dummyContainer = new DelegatingSpecimenContext();
        var loopTest = new LoopTest<Int64SequenceGenerator, long>(sut => (long)sut.Create(int64Request, dummyContainer));
        // Act & assert
        loopTest.Execute(10);
    }
}