using System;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest;

public class Int16SequenceGeneratorTest
{
    [Fact]
    public void SutIsSpecimenBuilder()
    {
        // Arrange
        // Act
        var sut = new Int16SequenceGenerator();
        // Assert
        Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
    }

    [Fact]
    public void CreateWithNullRequestWillReturnCorrectResult()
    {
        // Arrange
        var sut = new Int16SequenceGenerator();
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
        var sut = new Int16SequenceGenerator();
        // Act
        var dummyRequest = new object();
        sut.Create(dummyRequest, null);
        // Assert (no exception indicates success)
    }

    [Fact]
    public void CreateWithNonInt16RequestWillReturnCorrectResult()
    {
        // Arrange
        var nonInt16Request = new object();
        var sut = new Int16SequenceGenerator();
        // Act
        var dummyContainer = new DelegatingSpecimenContext();
        var result = sut.Create(nonInt16Request, dummyContainer);
        // Assert
        var expectedResult = NoSpecimen.Instance;
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void CreateWithInt16RequestWillReturnCorrectResult()
    {
        // Arrange
        var int16Request = typeof(short);
        var sut = new Int16SequenceGenerator();
        // Act
        var dummyContainer = new DelegatingSpecimenContext();
        var result = sut.Create(int16Request, dummyContainer);
        // Assert
        Assert.Equal((short)1, result);
    }

    [Fact]
    public void CreateWithInt16RequestWillReturnCorrectResultOnSecondCall()
    {
        // Arrange
        var int16Request = typeof(short);
        var dummyContainer = new DelegatingSpecimenContext();
        var loopTest = new LoopTest<Int16SequenceGenerator, short>(sut => (short)sut.Create(int16Request, dummyContainer));
        // Act & assert
        loopTest.Execute(2);
    }

    [Fact]
    public void CreateWithInt16RequestWillReturnCorrectResultOnTenthCall()
    {
        // Arrange
        var int16Request = typeof(short);
        var dummyContainer = new DelegatingSpecimenContext();
        var loopTest = new LoopTest<Int16SequenceGenerator, short>(sut => (short)sut.Create(int16Request, dummyContainer));
        // Act & assert
        loopTest.Execute(10);
    }
}