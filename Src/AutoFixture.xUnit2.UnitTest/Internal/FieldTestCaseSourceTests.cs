using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Xunit2.Internal;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.Xunit2.UnitTest.Internal
{
    public class FieldTestCaseSourceTests
    {
        public static IEnumerable<object[]> TestDataFieldWithMixedValues = new[]
        {
            new object[] { "hello", 1, new FieldHolder<string> { Field = "world" } },
            new object[] { "foo", 2, new FieldHolder<string> { Field = "bar" } },
            new object[] { "Han", 3, new FieldHolder<string> { Field = "Solo" } }
        };

        public static object NonEnumerableField = new object();

        [Fact]
        public void SutIsTestCaseSource()
        {
            // Arrange
            var sourceField = typeof(FieldTestCaseSourceTests)
                .GetField(nameof(TestDataFieldWithMixedValues));
            var sut = new FieldTestCaseSource(sourceField);

            // Assert
            Assert.IsAssignableFrom<ITestCaseSource>(sut);
        }

        [Fact]
        public void ThrowsWhenConstructedWithNullField()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new FieldTestCaseSource(null!));
        }

        [Fact]
        public void FieldIsCorrect()
        {
            // Arrange
            var expected = typeof(FieldTestCaseSourceTests)
                .GetField(nameof(TestDataFieldWithMixedValues));
            var sut = new FieldTestCaseSource(expected);

            // Act
            var result = sut.FieldInfo;

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ThrowsWhenInvokedWithNullTestMethod()
        {
            // Arrange
            var sourceField = typeof(FieldTestCaseSourceTests)
                .GetField(nameof(TestDataFieldWithMixedValues));
            var sut = new FieldTestCaseSource(sourceField);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => sut.GetTestCases(null!));
        }

        [Fact]
        public void ThrowsWhenSourceIsNotEnumerable()
        {
            // Arrange
            var sourceField = typeof(FieldTestCaseSourceTests)
                .GetField(nameof(NonEnumerableField));
            var sut = new FieldTestCaseSource(sourceField);
            var method = typeof(SampleTestType)
                .GetMethod(nameof(SampleTestType.TestMethodWithReferenceTypeParameter));

            // Act & Assert
            Assert.Throws<InvalidCastException>(() => sut.GetTestCases(method).ToArray());
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
            var sourceField = typeof(FieldTestCaseSourceTests)
                .GetField(nameof(TestDataFieldWithRecordValues));
            var sut = new FieldTestCaseSource(sourceField);
            var method = typeof(SampleTestType)
                .GetMethod(nameof(SampleTestType.TestMethodWithRecordTypeParameter));

            // Act
            var result = sut.GetTestCases(method).ToArray();

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