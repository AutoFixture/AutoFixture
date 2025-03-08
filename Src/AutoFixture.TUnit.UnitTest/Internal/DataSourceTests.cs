using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.TUnit.Internal;
using AutoFixture.TUnit.UnitTest.TestTypes;
using TUnit.Assertions.AssertConditions.Throws;

namespace AutoFixture.TUnit.UnitTest.Internal;

public class DataSourceTests
{
    [Test]
    public async Task SutIsTestDataSource()
    {
        // Arrange
        var sut = new DelegatingDataSource();

        // Assert
        await Assert.That(sut).IsAssignableTo<IDataSource>();
    }

    [Test]
    public async Task ReturnSingleEmptyArrayWhenMethodHasNoParameters()
    {
        // Arrange
        var sut = new DelegatingDataSource();
        var testMethod = typeof(SampleTestType)
            .GetMethod(nameof(SampleTestType.TestMethodWithoutParameters));

        // Act
        var result = sut.GetData(DataGeneratorMetadataHelper.CreateDataGeneratorMetadata(testMethod)).ToArray();

        // Assert
        var item = await Assert.That(result).HasSingleItem();

        await Assert.That(item).IsEmpty();
    }

    [Test]
    public async Task ThrowsWhenNoDataFoundForMethod()
    {
        // Arrange
        var sut = new DelegatingDataSource { TestData = null };
        var testMethod = typeof(SampleTestType)
            .GetMethod(nameof(SampleTestType.TestMethodWithSingleParameter));

        // Act & Assert
        await Assert.That(() => sut.GetData(DataGeneratorMetadataHelper.CreateDataGeneratorMetadata(testMethod)).ToArray()).ThrowsExactly<ArgumentNullException>();
    }

    [Test]
    public async Task ReturnSingleArrayWithSingleItemWhenMethodHasSingleParameter()
    {
        // Arrange
        var sut = new DelegatingDataSource
        {
            TestData = [["hello"]]
        };
        var testMethod = typeof(SampleTestType)
            .GetMethod(nameof(SampleTestType.TestMethodWithSingleParameter));

        // Act
        var result = sut.GetData(DataGeneratorMetadataHelper.CreateDataGeneratorMetadata(testMethod)).ToArray();

        // Assert
        var testData = await Assert.That(result).HasSingleItem();
        var argument = await Assert.That(testData).HasSingleItem();
        await Assert.That(argument).IsEqualTo("hello");
    }

    [Test]
    public async Task ReturnsArgumentsFittingTestParameters()
    {
        // Arrange
        var testData = new[]
        {
            new object[] { "hello", 16, 32.86d },
            new object[] { null, -1, -20.22 },
            new object[] { "one", 2 },
            new object[] { null },
            new object[] { },
        };
        var sut = new DelegatingDataSource { TestData = testData };
        var testMethod = typeof(SampleTestType)
            .GetMethod(nameof(SampleTestType.TestMethodWithMultipleParameters));

        // Act
        var actual = sut.GetData(DataGeneratorMetadataHelper.CreateDataGeneratorMetadata(testMethod)).ToArray();

        // Assert
        await Assert.That(actual.Length).IsEqualTo(testData.Length);

        await Assert.That(actual)
            .All()
            .Satisfy(
                assert => assert.Satisfies(y => y.Length,
                    y => y.IsBetween(0, 3).WithInclusiveBounds())
            );
    }

    [Test]
    public async Task ThrowsWhenTestDataContainsMoreArgumentsThanParameters()
    {
        // Arrange
        var testData = new[] { new object[] { "hello", 16, 32.86d, "extra" } };
        var sut = new DelegatingDataSource { TestData = testData };
        var testMethod = typeof(SampleTestType)
            .GetMethod(nameof(SampleTestType.TestMethodWithMultipleParameters));

        // Act & Assert
        await Assert.That(() => sut.GetData(DataGeneratorMetadataHelper.CreateDataGeneratorMetadata(testMethod)).ToArray()).ThrowsExactly<InvalidOperationException>();
    }
}