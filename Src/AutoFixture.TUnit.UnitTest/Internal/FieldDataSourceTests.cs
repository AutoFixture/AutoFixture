using System;
using System.Collections.Generic;
using AutoFixture.TUnit.UnitTest.TestTypes;
using TestTypeFoundation;

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
        public void SutIsTestDataSource()
        {
            // Arrange
            var sourceField = typeof(FieldDataSourceTests)
                .GetField(nameof(TestDataFieldWithMixedValues));
            var sut = new FieldDataSource(sourceField);

            // Assert
            Assert.IsAssignableFrom<IDataSource>(sut);
        }

        [Test]
        public void ThrowsWhenConstructedWithNullField()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => new FieldDataSource(null!));
        }

        [Test]
        public void FieldIsCorrect()
        {
            // Arrange
            var expected = typeof(FieldDataSourceTests)
                .GetField(nameof(TestDataFieldWithMixedValues));
            var sut = new FieldDataSource(expected);

            // Act
            var result = sut.FieldInfo;

            // Assert
            Assert.That(result).IsEqualTo(expected);
        }

        [Test]
        public void ThrowsWhenInvokedWithNullTestMethod()
        {
            // Arrange
            var sourceField = typeof(FieldDataSourceTests)
                .GetField(nameof(TestDataFieldWithMixedValues));
            var sut = new FieldDataSource(sourceField);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => sut.GetData(null!));
        }

        [Test]
        public void ThrowsWhenSourceIsNotEnumerable()
        {
            // Arrange
            var sourceField = typeof(FieldDataSourceTests)
                .GetField(nameof(NonEnumerableField));
            var sut = new FieldDataSource(sourceField);
            var method = typeof(SampleTestType)
                .GetMethod(nameof(SampleTestType.TestMethodWithReferenceTypeParameter));

            // Act & Assert
            Assert.Throws<InvalidCastException>(() => sut.GetData(method).ToArray());
        }

        [Test]
        public void GeneratesTestDataMatchingTestParameters()
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
            var result = sut.GetData(method).ToArray();

            // Assert
            Assert.That(result).IsEqualTo(expected);
        }

        public static IEnumerable<object[]> TestDataFieldWithRecordValues =
        [
            ["hello", 1, new RecordType<string>("world")],
            ["foo", 2, new RecordType<string>("bar")],
            ["Han", 3, new RecordType<string>("Solo")]
        ];
    }
}