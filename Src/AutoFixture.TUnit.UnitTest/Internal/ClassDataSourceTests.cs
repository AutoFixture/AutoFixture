using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.TUnit.Internal;
using AutoFixture.TUnit.UnitTest.TestTypes;
using TestTypeFoundation;
using TUnit.Assertions.AssertConditions.Throws;

namespace AutoFixture.TUnit.UnitTest.Internal;

public class ClassDataSourceTests
{
    [Test]
    public async Task SutIsTestDataSource()
    {
        // Arrange & Act
        var sut = new ClassDataSource(typeof(object), Array.Empty<object>());

        // Assert
        await Assert.That(sut).IsAssignableTo<IDataSource>();
    }

    [Test]
    public async Task ConstructorWithNullTypeThrows()
    {
        // Act & Assert
        await Assert.That(() => _ = new ClassDataSource(null!, Array.Empty<object>())).ThrowsExactly<ArgumentNullException>();
    }

    [Test]
    public async Task ConstructorWithNullParametersThrows()
    {
        // Act & Assert
        await Assert.That(() => _ = new ClassDataSource(typeof(object), null!)).ThrowsExactly<ArgumentNullException>();
    }

    [Test]
    public async Task TypeIsCorrect()
    {
        // Arrange
        var expected = typeof(object);
        var sut = new ClassDataSource(expected, Array.Empty<object>());

        // Act
        var result = sut.Type;

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    public async Task ParametersIsCorrect()
    {
        // Arrange
        var expected = new[] { new object() };
        var sut = new ClassDataSource(typeof(object), expected);

        // Act
        var result = sut.Parameters;

        // Assert
        await Assert.That(result).IsEquivalentTo(expected);
    }

    [Test]
    public async Task ThrowsWhenSourceIsNotEnumerable()
    {
        // Arrange
        var sut = new ClassDataSource(typeof(object), Array.Empty<object>());
        var method = typeof(SampleTestType)
            .GetMethod(nameof(SampleTestType.TestMethodWithReferenceTypeParameter));

        // Act & Assert
        await Assert.That(() => sut.GenerateDataSources(DataGeneratorMetadataHelper.CreateDataGeneratorMetadata(method)).ToArray()).ThrowsExactly<InvalidOperationException>();
    }

    [Test]
    public async Task GeneratesTestDatWithPrimitiveValues()
    {
        // Arrange
        var expected = new[]
        {
            new object[] { "hello", 1, new RecordType<string>("world") },
            new object[] { "foo", 2, new RecordType<string>("bar") },
            new object[] { "Han", 3, new RecordType<string>("Solo") }
        };
        var sut = new ClassDataSource(typeof(TestSourceWithMixedValues), Array.Empty<object>());
        var method = typeof(SampleTestType)
            .GetMethod(nameof(SampleTestType.TestMethodWithReferenceTypeParameter));

        // Act
        var actual = sut.GenerateDataSources(DataGeneratorMetadataHelper.CreateDataGeneratorMetadata(method))
            .Select(x => x())
            .ToArray();

        // Assert
        await Assert.That(actual).IsEquivalentTo(expected);
    }

    private class TestSourceWithMixedValues : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return ["hello", 1, new RecordType<string>("world")];
            yield return ["foo", 2, new RecordType<string>("bar")];
            yield return ["Han", 3, new RecordType<string>("Solo")];
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }

    [Test]
    public async Task ThrowsWhenConstructorParametersDontMatch()
    {
        // Arrange
        var parameters = new object[] { "a", 1 };
        var sut = new ClassDataSource(typeof(TestSourceWithMixedValues), parameters);
        var method = typeof(SampleTestType)
            .GetMethod(nameof(SampleTestType.TestMethodWithReferenceTypeParameter));

        // Act & Assert
        await Assert.That(() => sut.GenerateDataSources(DataGeneratorMetadataHelper.CreateDataGeneratorMetadata(method)).ToArray()).ThrowsException();
    }

    [Test]
    public async Task AppliesExpectedConstructorParameters()
    {
        // Arrange
        object[] parameters = [new object[] { "y", 25 }];
        var sut = new ClassDataSource(typeof(DelegatingTestData), parameters);
        var method = typeof(SampleTestType)
            .GetMethod(nameof(SampleTestType.TestMethodWithReferenceTypeParameter));

        // Act
        var result = sut.GenerateDataSources(DataGeneratorMetadataHelper.CreateDataGeneratorMetadata(method)).ToArray();

        // Assert
        await Assert.That(result.Single()).IsEquivalentTo(new object[] { "y", 25 });
    }
}