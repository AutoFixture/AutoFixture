using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.TUnit.Internal;
using AutoFixture.TUnit.UnitTest.TestTypes;
using TestTypeFoundation;
using TUnit.Assertions.AssertConditions.Throws;

namespace AutoFixture.TUnit.UnitTest.Internal;

public class PropertyDataSourceTests
{
    [Test]
    public async Task SutIsTestDataSource()
    {
        // Arrange
        var sourceProperty = typeof(PropertyDataSourceTests)
            .GetProperty(nameof(TestDataPropertyWithMixedValues));
        var sut = new PropertyDataSource(sourceProperty);

        // Assert
        await Assert.That(sut).IsAssignableTo<IDataSource>();
    }

    public static IEnumerable<object[]> TestDataPropertyWithMixedValues =>
    [
        ["hello", 1, new PropertyHolder<string> { Property = "world" }],
        ["foo", 2, new PropertyHolder<string> { Property = "bar" }],
        ["Han", 3, new PropertyHolder<string> { Property = "Solo" }]
    ];

    public static object NonEnumerableProperty => new object();

    [Test]
    public async Task ThrowsWhenConstructedWithNullProperty()
    {
        // Act & Assert
        await Assert.That(() => new PropertyDataSource(null!)).ThrowsExactly<ArgumentNullException>();
    }

    [Test]
    public async Task PropertyIsCorrect()
    {
        // Arrange
        var expected = typeof(PropertyDataSourceTests)
            .GetProperty(nameof(TestDataPropertyWithMixedValues));
        var sut = new PropertyDataSource(expected);

        // Act
        var result = sut.PropertyInfo;

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    public async Task ThrowsWhenInvokedWithNullTestMethod()
    {
        // Arrange
        var sourceProperty = typeof(PropertyDataSourceTests)
            .GetProperty(nameof(TestDataPropertyWithMixedValues));
        var sut = new PropertyDataSource(sourceProperty);

        // Act & Assert
        await Assert.That(() => sut.GenerateDataSources(null!)).ThrowsExactly<ArgumentNullException>();
    }

    [Test]
    public async Task ThrowsWhenSourceIsNotEnumerable()
    {
        // Arrange
        var sourceProperty = typeof(PropertyDataSourceTests)
            .GetProperty(nameof(NonEnumerableProperty));
        var sut = new PropertyDataSource(sourceProperty);
        var method = typeof(SampleTestType)
            .GetMethod(nameof(SampleTestType.TestMethodWithReferenceTypeParameter));

        // Act & Assert
        await Assert.That(() => sut.GenerateDataSources(DataGeneratorMetadataHelper.CreateDataGeneratorMetadata(method))
            
            .ToArray()).ThrowsExactly<InvalidCastException>();
    }

    [Test]
    public async Task GeneratesTestDataMatchingTestParameters()
    {
        // Arrange
        var expected = new[]
        {
            new object[] { "hello", 1, new RecordType<string>("world") },
            new object[] { "foo", 2, new RecordType<string>("bar") },
            new object[] { "Han", 3, new RecordType<string>("Solo") }
        };
        var sourceProperty = typeof(PropertyDataSourceTests)
            .GetProperty(nameof(TestDataPropertyWithRecordValues));
        var sut = new PropertyDataSource(sourceProperty);
        var method = typeof(SampleTestType)
            .GetMethod(nameof(SampleTestType.TestMethodWithRecordTypeParameter));

        // Act
        var result = sut.GenerateDataSources(DataGeneratorMetadataHelper.CreateDataGeneratorMetadata(method))
            .Select(x => x())
            .ToArray();

        // Assert
        await Assert.That(result).IsEquivalentTo(expected);
    }

    public static IEnumerable<object[]> TestDataPropertyWithRecordValues =>
    [
        ["hello", 1, new RecordType<string>("world")],
        ["foo", 2, new RecordType<string>("bar")],
        ["Han", 3, new RecordType<string>("Solo")]
    ];

    [Test]
    public async Task ReturnsNullArguments()
    {
        // Arrange
        var expected = new[]
        {
            new object[] { null, 1, null },
            new object[] { null, 2, null },
            new object[] { null, 3, null }
        };
        var sourceProperty = typeof(PropertyDataSourceTests)
            .GetProperty(nameof(TestDataPropertyWithNullValues));
        var sut = new PropertyDataSource(sourceProperty);
        var testMethod = typeof(SampleTestType)
            .GetMethod(nameof(SampleTestType.TestMethodWithRecordTypeParameter));

        // Act
        var result = sut.GenerateDataSources(DataGeneratorMetadataHelper.CreateDataGeneratorMetadata(testMethod))
            .Select(x => x())
            .ToArray();

        // Assert
        await Assert.That(result).IsEquivalentTo(expected);
    }

    public static IEnumerable<object[]> TestDataPropertyWithNullValues =>
    [
        [null, 1, null],
        [null, 2, null],
        [null, 3, null]
    ];
}