using System;
using AutoFixture.Xunit2.Internal;
using Xunit;

namespace AutoFixture.Xunit2.UnitTest.Internal;

public class InlineTestCaseSourceTests
{
    [Fact]
    public void SutIsTestCaseSource()
    {
        // Arrange
        // Act
        var sut = new InlineTestCaseSource(Array.Empty<object>());
        // Assert
        Assert.IsAssignableFrom<ITestCaseSource>(sut);
    }

    [Fact]
    public void InitializeWithNullValuesThrows()
    {
        // Arrange
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new InlineTestCaseSource(null));
    }

    [Fact]
    public void ValuesIsCorrect()
    {
        // Arrange
        var expectedValues = Array.Empty<object>();
        var sut = new InlineTestCaseSource(expectedValues);
        // Act
        var result = sut.Values;
        // Assert
        Assert.Equal(expectedValues, result);
    }

    [Fact]
    public void GetTestCasesWithNullMethodThrows()
    {
        // Arrange
        var sut = new InlineTestCaseSource(Array.Empty<object>());
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            sut.GetTestCases(null));
    }

    [Fact]
    public void SourceThrowsWhenArgumentCountExceedParameterCount()
    {
        // Arrange
        var values = new object[] { "aloha", 42, 12.3d, "extra" };
        var sut = new InlineTestCaseSource(values);
        var testMethod = typeof(SampleTestType)
            .GetMethod(nameof(SampleTestType.TestMethodWithMultipleParameters));

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            sut.GetTestCases(testMethod));
    }

    [Fact]
    public void ReturnsTestCaseWhenArgumentCountMatchesParameterCount()
    {
        // Arrange
        var values = new object[] { "aloha", 42, 12.3d };
        var sut = new InlineTestCaseSource(values);
        var testMethod = typeof(SampleTestType)
            .GetMethod(nameof(SampleTestType.TestMethodWithMultipleParameters));

        // Act
        var result = sut.GetTestCases(testMethod);

        // Assert
        var testCase = Assert.Single(result);
        Assert.Equal(values, testCase);
    }

    [Fact]
    public void ReturnsAllArgumentsWhenArgumentCountLessThanParameterCount()
    {
        // Arrange
        var values = new object[] { "aloha", 42 };
        var sut = new InlineTestCaseSource(values);
        var testMethod = typeof(SampleTestType)
            .GetMethod(nameof(SampleTestType.TestMethodWithMultipleParameters));

        // Act
        var result = sut.GetTestCases(testMethod);

        // Assert
        var testCase = Assert.Single(result);
        Assert.Equal(values, testCase);
    }
}