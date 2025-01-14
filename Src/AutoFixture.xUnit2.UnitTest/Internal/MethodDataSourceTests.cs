using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Xunit2.Internal;
using AutoFixture.Xunit2.UnitTest.TestTypes;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.Xunit2.UnitTest.Internal
{
    public class MethodDataSourceTests
    {
        public static IEnumerable<object[]> GetTestDataFieldWithMixedValues()
        {
            yield return new object[] { "hello", 1, new RecordType<string>("world") };
            yield return new object[] { "foo", 2, new RecordType<string>("bar") };
            yield return new object[] { "Han", 3, new RecordType<string>("Solo") };
        }

        [Fact]
        public void SutIsTestDataSource()
        {
            // Arrange
            var methodInfo = typeof(MethodDataSourceTests)
                .GetMethod(nameof(this.SutIsTestDataSource));

            // Act
            var sut = new MethodDataSource(methodInfo);

            // Assert
            Assert.IsAssignableFrom<DataSource>(sut);
        }

        [Fact]
        public void ThrowsWhenMethodInfoIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new MethodDataSource(null!));
        }

        [Fact]
        public void ThrowsWhenArgumentsIsNull()
        {
            // Arrange
            var methodInfo = typeof(MethodDataSourceTests)
                .GetMethod(nameof(this.SutIsTestDataSource));

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new MethodDataSource(methodInfo, null!));
        }

        [Fact]
        public void ConstructorSetsProperties()
        {
            // Arrange
            var methodInfo = typeof(MethodDataSourceTests)
                .GetMethod(nameof(this.SutIsTestDataSource));
            var arguments = new[] { new object() };

            // Act
            var sut = new MethodDataSource(methodInfo, arguments);

            // Assert
            Assert.Equal(methodInfo, sut.MethodInfo);
            Assert.Equal(arguments, sut.Arguments);
        }

        [Fact]
        public void GetTestDataInvokesMethodInfo()
        {
            // Arrange
            var expected = new[]
            {
                new object[] { "hello", 1, new RecordType<string>("world") },
                new object[] { "foo", 2, new RecordType<string>("bar") },
                new object[] { "Han", 3, new RecordType<string>("Solo") }
            };
            var testDataSource = typeof(MethodDataSourceTests)
                .GetMethod(nameof(this.GetTestDataFieldWithMixedValues));
            var testData = typeof(SampleTestType)
                .GetMethod(nameof(SampleTestType.TestMethodWithReferenceTypeParameter));
            var sut = new MethodDataSource(testDataSource);

            // Act
            var result = sut.GetData(testData);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ThrowsWhenMemberDoesNotReturnAnEnumerableValue()
        {
            // Arrange
            var dataSource = typeof(MethodDataSourceTests)
                .GetMethod(nameof(NonEnumerableTestData));
            var testData = typeof(SampleTestType)
                .GetMethod(nameof(SampleTestType.TestMethodWithReferenceTypeParameter));
            var sut = new MethodDataSource(dataSource);

            // Act & Assert
            Assert.Throws<InvalidCastException>(
                () => sut.GetData(testData).ToArray());
        }

        public static object NonEnumerableTestData() => new();
    }
}