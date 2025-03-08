using System;
using System.Threading.Tasks;
using AutoFixture.TUnit.Internal;
using AutoFixture.TUnit.UnitTest.TestTypes;
using TUnit.Assertions.AssertConditions.Throws;

namespace AutoFixture.TUnit.UnitTest.Internal;

public class InlineDataSourceTests
{
    [Test]
    public async Task SutIsTestDataSource()
    {
        // Arrange
        // Act
        var sut = new InlineDataSource(Array.Empty<object>());
        // Assert
        await Assert.That(sut).IsTypeOf<IDataSource>();
    }

    [Test]
    public async Task InitializeWithNullValuesThrows()
    {
        // Arrange
        // Act & Assert
        await Assert.That(() =>
            new InlineDataSource(null)).ThrowsExactly<ArgumentNullException>();
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
    public async Task GetTestDataWithNullMethodThrows()
    {
        // Arrange
        var sut = new InlineDataSource(Array.Empty<object>());
        // Act & Assert
        await Assert.That(() =>
            sut.GenerateDataSources(null)).ThrowsExactly<ArgumentNullException>();
    }

    [Test]
    public async Task SourceThrowsWhenArgumentCountExceedParameterCount()
    {
        // Arrange
        var values = new object[] { "aloha", 42, 12.3d, "extra" };
        var sut = new InlineDataSource(values);
        var testMethod = typeof(SampleTestType)
            .GetMethod(nameof(SampleTestType.TestMethodWithMultipleParameters));

        // Act & Assert
        await Assert.That(() =>
            sut.GenerateDataSources(testMethod)).ThrowsExactly<InvalidOperationException>();
    }

    [Test]
    public async Task ReturnsTestDataWhenArgumentCountMatchesParameterCount()
    {
        // Arrange
        var values = new object[] { "aloha", 42, 12.3d };
        var sut = new InlineDataSource(values);
        var testMethod = typeof(SampleTestType)
            .GetMethod(nameof(SampleTestType.TestMethodWithMultipleParameters));

        // Act
        var result = sut.GenerateDataSources(testMethod);

        // Assert
        var testData = await Assert.That(result).HasSingleItem();
        await Assert.That(testData).IsEqualTo(values);
    }

    [Test]
    public async Task ReturnsAllArgumentsWhenArgumentCountLessThanParameterCount()
    {
        // Arrange
        var values = new object[] { "aloha", 42 };
        var sut = new InlineDataSource(values);
        var testMethod = typeof(SampleTestType)
            .GetMethod(nameof(SampleTestType.TestMethodWithMultipleParameters));

        // Act
        var result = sut.GenerateDataSources(testMethod);

        // Assert
        var testData = await Assert.That(result).HasSingleItem();
        await Assert.That(testData).IsEqualTo(values);
    }
}