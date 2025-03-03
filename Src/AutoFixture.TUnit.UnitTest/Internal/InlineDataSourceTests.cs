using System;
using System.Threading.Tasks;
using AutoFixture.TUnit.Internal;
using AutoFixture.TUnit.UnitTest.TestTypes;

namespace AutoFixture.TUnit.UnitTest.Internal;

public class InlineDataSourceTests
{
    [Test]
    public void SutIsTestDataSource()
    {
        // Arrange
        // Act
        var sut = new InlineDataSource(Array.Empty<object>());
        // Assert
        Assert.IsAssignableFrom<IDataSource>(sut);
    }

    [Test]
    public void InitializeWithNullValuesThrows()
    {
        // Arrange
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new InlineDataSource(null));
    }

    [Test]
    public async Task ValuesIsCorrect()
    {
        // Arrange
        var expectedValues = Array.Empty<object>();
        var sut = new InlineDataSource(expectedValues);
        // Act
        var result = sut.Values;
        // Assert
        await Assert.That(result).IsEqualTo(expectedValues);
    }

    [Test]
    public void GetTestDataWithNullMethodThrows()
    {
        // Arrange
        var sut = new InlineDataSource(Array.Empty<object>());
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            sut.GetData(null));
    }

    [Test]
    public void SourceThrowsWhenArgumentCountExceedParameterCount()
    {
        // Arrange
        var values = new object[] { "aloha", 42, 12.3d, "extra" };
        var sut = new InlineDataSource(values);
        var testMethod = typeof(SampleTestType)
            .GetMethod(nameof(SampleTestType.TestMethodWithMultipleParameters));

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            sut.GetData(testMethod));
    }

    [Test]
    public void ReturnsTestDataWhenArgumentCountMatchesParameterCount()
    {
        // Arrange
        var values = new object[] { "aloha", 42, 12.3d };
        var sut = new InlineDataSource(values);
        var testMethod = typeof(SampleTestType)
            .GetMethod(nameof(SampleTestType.TestMethodWithMultipleParameters));

        // Act
        var result = sut.GetData(testMethod);

        // Assert
        var testData = Assert.Single(result);
        Assert.That(testData).IsEqualTo(values);
    }

    [Test]
    public void ReturnsAllArgumentsWhenArgumentCountLessThanParameterCount()
    {
        // Arrange
        var values = new object[] { "aloha", 42 };
        var sut = new InlineDataSource(values);
        var testMethod = typeof(SampleTestType)
            .GetMethod(nameof(SampleTestType.TestMethodWithMultipleParameters));

        // Act
        var result = sut.GetData(testMethod);

        // Assert
        var testData = Assert.Single(result);
        Assert.That(testData).IsEqualTo(values);
    }
}