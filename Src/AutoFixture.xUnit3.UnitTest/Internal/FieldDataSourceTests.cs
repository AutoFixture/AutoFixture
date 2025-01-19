using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Xunit3.Internal;
using AutoFixture.Xunit3.UnitTest.TestTypes;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.Xunit3.UnitTest.Internal
{
    public class FieldDataSourceTests
    {
        public static IEnumerable<object[]> TestDataFieldWithMixedValues = new[]
        {
            new object[] { "hello", 1, new FieldHolder<string> { Field = "world" } },
            new object[] { "foo", 2, new FieldHolder<string> { Field = "bar" } },
            new object[] { "Han", 3, new FieldHolder<string> { Field = "Solo" } }
        };

        public static object NonEnumerableField = new object();

        [Fact]
        public void SutIsTestDataSource()
        {
            // Arrange
            var sourceField = typeof(FieldDataSourceTests)
                .GetField(nameof(TestDataFieldWithMixedValues));
            var sut = new FieldDataSource(sourceField);

            // Assert
            Assert.IsAssignableFrom<IDataSource>(sut);
        }

        [Fact]
        public void ThrowsWhenConstructedWithNullField()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => new FieldDataSource(null!));
        }

        [Fact]
        public void FieldIsCorrect()
        {
            // Arrange
            var expected = typeof(FieldDataSourceTests)
                .GetField(nameof(TestDataFieldWithMixedValues));
            var sut = new FieldDataSource(expected);

            // Act
            var result = sut.FieldInfo;

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
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

        [Fact]
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

        [Fact]
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
            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> TestDataFieldWithRecordValues = new[]
        {
            new object[] { "hello", 1, new RecordType<string>("world") },
            new object[] { "foo", 2, new RecordType<string>("bar") },
            new object[] { "Han", 3, new RecordType<string>("Solo") }
        };
    }
}