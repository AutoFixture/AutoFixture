using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.TUnit.Internal;
using AutoFixture.TUnit.UnitTest.TestTypes;
using TestTypeFoundation;
using TUnit.Assertions.AssertConditions.Throws;

namespace AutoFixture.TUnit.UnitTest.Internal
{
    public class FieldDataSourceTests
    {
        public static IEnumerable<object[]> TestDataFieldWithMixedValues =
        [
            ["hello", 1, new FieldHolder<string> { Field = "world" }],
            ["foo", 2, new FieldHolder<string> { Field = "bar" }],
            ["Han", 3, new FieldHolder<string> { Field = "Solo" }]
        ];

        public static object NonEnumerableField = new object();

        [Test]
        public async Task SutIsTestDataSource()
        {
            // Arrange
            var sourceField = typeof(FieldDataSourceTests)
                .GetField(nameof(TestDataFieldWithMixedValues));
            var sut = new FieldDataSource(sourceField);

            // Assert
            await Assert.That(sut).IsAssignableTo<IDataSource>();
        }

        [Test]
        public async Task ThrowsWhenConstructedWithNullField()
        {
            // Act & Assert
            await Assert.That(() => new FieldDataSource(null!)).ThrowsExactly<ArgumentNullException>();
        }

        [Test]
        public async Task FieldIsCorrect()
        {
            // Arrange
            var expected = typeof(FieldDataSourceTests)
                .GetField(nameof(TestDataFieldWithMixedValues));
            var sut = new FieldDataSource(expected);

            // Act
            var result = sut.FieldInfo;

            // Assert
            await Assert.That(result).IsEqualTo(expected);
        }

        [Test]
        public async Task ThrowsWhenInvokedWithNullTestMethod()
        {
            // Arrange
            var sourceField = typeof(FieldDataSourceTests)
                .GetField(nameof(TestDataFieldWithMixedValues));
            var sut = new FieldDataSource(sourceField);

            // Act & Assert
            await Assert.That(() => sut.GenerateDataSources(null!)).ThrowsExactly<ArgumentNullException>();
        }

        [Test]
        public async Task ThrowsWhenSourceIsNotEnumerable()
        {
            // Arrange
            var sourceField = typeof(FieldDataSourceTests)
                .GetField(nameof(NonEnumerableField));
            var sut = new FieldDataSource(sourceField);
            var method = typeof(SampleTestType)
                .GetMethod(nameof(SampleTestType.TestMethodWithReferenceTypeParameter));

            // Act & Assert
            await Assert.That(() => sut.GenerateDataSources(DataGeneratorMetadataHelper.CreateDataGeneratorMetadata(method)).ToArray()).ThrowsExactly<InvalidCastException>();
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
            var sourceField = typeof(FieldDataSourceTests)
                .GetField(nameof(TestDataFieldWithRecordValues));
            var sut = new FieldDataSource(sourceField);
            var method = typeof(SampleTestType)
                .GetMethod(nameof(SampleTestType.TestMethodWithRecordTypeParameter));

            // Act
            var result = sut.GenerateDataSources(DataGeneratorMetadataHelper.CreateDataGeneratorMetadata(method))
                .Select(x => x())
                .ToArray();

            // Assert
            await Assert.That(result).IsEqualTo(expected);
        }

        public static IEnumerable<object[]> TestDataFieldWithRecordValues =
        [
            ["hello", 1, new RecordType<string>("world")],
            ["foo", 2, new RecordType<string>("bar")],
            ["Han", 3, new RecordType<string>("Solo")]
        ];
    }
}