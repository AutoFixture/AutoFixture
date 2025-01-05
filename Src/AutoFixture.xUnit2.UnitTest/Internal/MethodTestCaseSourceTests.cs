using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Xunit2.Internal;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.Xunit2.UnitTest.Internal
{
    public class MethodTestCaseSourceTests
    {
        public static IEnumerable<object[]> GetTestDataFieldWithMixedValues()
        {
            yield return new object[] { "hello", 1, new RecordType<string>("world") };
            yield return new object[] { "foo", 2, new RecordType<string>("bar") };
            yield return new object[] { "Han", 3, new RecordType<string>("Solo") };
        }

        [Fact]
        public void SutIsTestCaseSource()
        {
            // Arrange
            var methodInfo = typeof(MethodTestCaseSourceTests)
                .GetMethod(nameof(this.SutIsTestCaseSource));

            // Act
            var sut = new MethodTestCaseSource(methodInfo);

            // Assert
            Assert.IsAssignableFrom<TestCaseSource>(sut);
        }

        [Fact]
        public void ThrowsWhenMethodInfoIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new MethodTestCaseSource(null!));
        }

        [Fact]
        public void ThrowsWhenArgumentsIsNull()
        {
            // Arrange
            var methodInfo = typeof(MethodTestCaseSourceTests)
                .GetMethod(nameof(this.SutIsTestCaseSource));

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new MethodTestCaseSource(methodInfo, null!));
        }

        [Fact]
        public void ConstructorSetsProperties()
        {
            // Arrange
            var methodInfo = typeof(MethodTestCaseSourceTests)
                .GetMethod(nameof(this.SutIsTestCaseSource));
            var arguments = new[] { new object() };

            // Act
            var sut = new MethodTestCaseSource(methodInfo, arguments);

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
            var testDataSource = typeof(MethodTestCaseSourceTests)
                .GetMethod(nameof(this.GetTestDataFieldWithMixedValues));
            var testData = typeof(SampleTestType)
                .GetMethod(nameof(SampleTestType.TestMethodWithReferenceTypeParameter));
            var sut = new MethodTestCaseSource(testDataSource);

            // Act
            var result = sut.GetTestCases(testData);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ThrowsWhenMemberDoesNotReturnAnEnumerableValue()
        {
            // Arrange
            var dataSource = typeof(MethodTestCaseSourceTests)
                .GetMethod(nameof(NonEnumerableTestData));
            var testData = typeof(SampleTestType)
                .GetMethod(nameof(SampleTestType.TestMethodWithReferenceTypeParameter));
            var sut = new MethodTestCaseSource(dataSource);

            // Act & Assert
            Assert.Throws<InvalidCastException>(() => sut.GetTestCases(testData).ToArray());
        }

        public static object NonEnumerableTestData() => new();
    }
}