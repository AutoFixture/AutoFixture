using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Xunit2.Internal;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.Xunit2.UnitTest.Internal
{
    public class PropertyTestCaseSourceTests
    {
        [Fact]
        public void SutIsTestCaseSource()
        {
            // Arrange
            var sourceProperty = typeof(PropertyTestCaseSourceTests)
                .GetProperty(nameof(TestDataPropertyWithMixedValues));
            var sut = new PropertyTestCaseSource(sourceProperty);

            // Assert
            Assert.IsAssignableFrom<ITestCaseSource>(sut);
        }

        public static IEnumerable<object[]> TestDataPropertyWithMixedValues => new[]
        {
            new object[] { "hello", 1, new PropertyHolder<string> { Property = "world" } },
            new object[] { "foo", 2, new PropertyHolder<string> { Property = "bar" } },
            new object[] { "Han", 3, new PropertyHolder<string> { Property = "Solo" } }
        };

        public static object NonEnumerableProperty => new object();

        [Fact]
        public void ThrowsWhenConstructedWithNullProperty()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => new PropertyTestCaseSource(null!));
        }

        [Fact]
        public void PropertyIsCorrect()
        {
            // Arrange
            var expected = typeof(PropertyTestCaseSourceTests)
                .GetProperty(nameof(TestDataPropertyWithMixedValues));
            var sut = new PropertyTestCaseSource(expected);

            // Act
            var result = sut.PropertyInfo;

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ThrowsWhenInvokedWithNullTestMethod()
        {
            // Arrange
            var sourceProperty = typeof(PropertyTestCaseSourceTests)
                .GetProperty(nameof(TestDataPropertyWithMixedValues));
            var sut = new PropertyTestCaseSource(sourceProperty);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => sut.GetTestCases(null!));
        }

        [Fact]
        public void ThrowsWhenSourceIsNotEnumerable()
        {
            // Arrange
            var sourceProperty = typeof(PropertyTestCaseSourceTests)
                .GetProperty(nameof(NonEnumerableProperty));
            var sut = new PropertyTestCaseSource(sourceProperty);
            var method = typeof(SampleTestType)
                .GetMethod(nameof(SampleTestType.TestMethodWithReferenceTypeParameter));

            // Act & Assert
            Assert.Throws<InvalidCastException>(
                () => sut.GetTestCases(method).ToArray());
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
            var sourceProperty = typeof(PropertyTestCaseSourceTests)
                .GetProperty(nameof(TestDataPropertyWithRecordValues));
            var sut = new PropertyTestCaseSource(sourceProperty);
            var method = typeof(SampleTestType)
                .GetMethod(nameof(SampleTestType.TestMethodWithRecordTypeParameter));

            // Act
            var result = sut.GetTestCases(method).ToArray();

            // Assert
            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> TestDataPropertyWithRecordValues => new[]
        {
            new object[] { "hello", 1, new RecordType<string>("world") },
            new object[] { "foo", 2, new RecordType<string>("bar") },
            new object[] { "Han", 3, new RecordType<string>("Solo") }
        };
    }
}