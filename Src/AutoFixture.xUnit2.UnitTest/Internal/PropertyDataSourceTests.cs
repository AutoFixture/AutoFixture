using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Xunit2.Internal;
using AutoFixture.Xunit2.UnitTest.TestTypes;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.Xunit2.UnitTest.Internal
{
    public class PropertyDataSourceTests
    {
        [Fact]
        public void SutIsTestDataSource()
        {
            // Arrange
            var sourceProperty = typeof(PropertyDataSourceTests)
                .GetProperty(nameof(TestDataPropertyWithMixedValues));
            var sut = new PropertyDataSource(sourceProperty);

            // Assert
            Assert.IsAssignableFrom<IDataSource>(sut);
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
                () => new PropertyDataSource(null!));
        }

        [Fact]
        public void PropertyIsCorrect()
        {
            // Arrange
            var expected = typeof(PropertyDataSourceTests)
                .GetProperty(nameof(TestDataPropertyWithMixedValues));
            var sut = new PropertyDataSource(expected);

            // Act
            var result = sut.PropertyInfo;

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ThrowsWhenInvokedWithNullTestMethod()
        {
            // Arrange
            var sourceProperty = typeof(PropertyDataSourceTests)
                .GetProperty(nameof(TestDataPropertyWithMixedValues));
            var sut = new PropertyDataSource(sourceProperty);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => sut.GetData(null!));
        }

        [Fact]
        public void ThrowsWhenSourceIsNotEnumerable()
        {
            // Arrange
            var sourceProperty = typeof(PropertyDataSourceTests)
                .GetProperty(nameof(NonEnumerableProperty));
            var sut = new PropertyDataSource(sourceProperty);
            var method = typeof(SampleTestType)
                .GetMethod(nameof(SampleTestType.TestMethodWithReferenceTypeParameter));

            // Act & Assert
            Assert.Throws<InvalidCastException>(
                () => sut.GetData(method).ToArray());
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
            var sourceProperty = typeof(PropertyDataSourceTests)
                .GetProperty(nameof(TestDataPropertyWithRecordValues));
            var sut = new PropertyDataSource(sourceProperty);
            var method = typeof(SampleTestType)
                .GetMethod(nameof(SampleTestType.TestMethodWithRecordTypeParameter));

            // Act
            var result = sut.GetData(method).ToArray();

            // Assert
            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> TestDataPropertyWithRecordValues => new[]
        {
            new object[] { "hello", 1, new RecordType<string>("world") },
            new object[] { "foo", 2, new RecordType<string>("bar") },
            new object[] { "Han", 3, new RecordType<string>("Solo") }
        };

        [Fact]
        public void ReturnsNullArguments()
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
            var result = sut.GetData(testMethod).ToArray();

            // Assert
            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> TestDataPropertyWithNullValues => new[]
        {
            new object[] { null, 1, null },
            new object[] { null, 2, null },
            new object[] { null, 3, null }
        };
    }
}