using System;
using System.Linq;
using AutoFixture.Xunit2.Internal;
using Xunit;

namespace AutoFixture.Xunit2.UnitTest.Internal
{
    public class TestCaseSourceTests
    {
        [Fact]
        public void SutIsTestCaseSource()
        {
            // Arrange
            var sut = new DelegatingTestCaseSource();

            // Assert
            Assert.IsAssignableFrom<ITestCaseSource>(sut);
        }

        [Fact]
        public void ThrowsWhenInvokedWithNullMethodInfo()
        {
            // Arrange
            var sut = new DelegatingTestCaseSource();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => sut.GetTestCases(null));
        }

        [Fact]
        public void ReturnSingleEmptyArrayWhenMethodHasNoParameters()
        {
            // Arrange
            var sut = new DelegatingTestCaseSource();
            var testMethod = typeof(SampleTestType)
                .GetMethod(nameof(SampleTestType.TestMethodWithoutParameters));

            // Act
            var result = sut.GetTestCases(testMethod).ToArray();

            // Assert
            var item = Assert.Single(result);
            Assert.Empty(item);
        }

        [Fact]
        public void ThrowsWhenNoDataFoundForMethod()
        {
            // Arrange
            var sut = new DelegatingTestCaseSource { TestCases = null };
            var testMethod = typeof(SampleTestType)
                .GetMethod(nameof(SampleTestType.TestMethodWithSingleParameter));

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => sut.GetTestCases(testMethod).ToArray());
        }

        [Fact]
        public void ReturnSingleArrayWithSingleItemWhenMethodHasSingleParameter()
        {
            // Arrange
            var sut = new DelegatingTestCaseSource
            {
                TestCases = new[] { new object[] { "hello" } }
            };
            var testMethod = typeof(SampleTestType)
                .GetMethod(nameof(SampleTestType.TestMethodWithSingleParameter));

            // Act
            var result = sut.GetTestCases(testMethod).ToArray();

            // Assert
            var testCase = Assert.Single(result);
            var argument = Assert.Single(testCase);
            Assert.Equal("hello", argument);
        }

        [Fact]
        public void ReturnsArgumentsFittingTestParameters()
        {
            // Arrange
            var testCases = new[]
            {
                new object[] { "hello", 16, 32.86d },
                new object[] { null, -1, -20.22 },
                new object[] { "one", 2 },
                new object[] { null },
                new object[] { },
            };
            var sut = new DelegatingTestCaseSource { TestCases = testCases };
            var testMethod = typeof(SampleTestType)
                .GetMethod(nameof(SampleTestType.TestMethodWithMultipleParameters));

            // Act
            var actual = sut.GetTestCases(testMethod).ToArray();

            // Assert
            Assert.Equal(testCases.Length, actual.Length);
            Assert.All(actual, x => Assert.InRange(x.Length, 0, 3));
        }

        [Fact]
        public void ThrowsWhenTestDataContainsMoreArgumentsThanParameters()
        {
            // Arrange
            var testCases = new[]
            {
                new object[] { "hello", 16, 32.86d, "extra" },
            };
            var sut = new DelegatingTestCaseSource { TestCases = testCases };
            var testMethod = typeof(SampleTestType)
                .GetMethod(nameof(SampleTestType.TestMethodWithMultipleParameters));

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => sut.GetTestCases(testMethod).ToArray());
        }
    }
}
