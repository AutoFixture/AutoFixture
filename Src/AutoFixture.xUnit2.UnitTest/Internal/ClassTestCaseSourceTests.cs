using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Xunit2.Internal;
using AutoFixture.Xunit2.UnitTest.TestTypes;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.Xunit2.UnitTest.Internal
{
    public class ClassTestCaseSourceTests
    {
        [Fact]
        public void SutIsTestCaseSource()
        {
            // Arrange & Act
            var sut = new ClassTestCaseSource(typeof(object), Array.Empty<object>());

            // Assert
            Assert.IsAssignableFrom<ITestCaseSource>(sut);
        }

        [Fact]
        public void ConstructorWithNullTypeThrows()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => _ = new ClassTestCaseSource(null!, Array.Empty<object>()));
        }

        [Fact]
        public void ConstructorWithNullParametersThrows()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => _ = new ClassTestCaseSource(typeof(object), null!));
        }

        [Fact]
        public void TypeIsCorrect()
        {
            // Arrange
            var expected = typeof(object);
            var sut = new ClassTestCaseSource(expected, Array.Empty<object>());

            // Act
            var result = sut.Type;

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ParametersIsCorrect()
        {
            // Arrange
            var expected = new[] { new object() };
            var sut = new ClassTestCaseSource(typeof(object), expected);

            // Act
            var result = sut.Parameters;

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ThrowsWhenSourceIsNotEnumerable()
        {
            // Arrange
            var sut = new ClassTestCaseSource(typeof(object), Array.Empty<object>());
            var method = typeof(SampleTestType)
                .GetMethod(nameof(SampleTestType.TestMethodWithReferenceTypeParameter));

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => sut.GetTestCases(method).ToArray());
        }

        [Fact]
        public void GeneratesTestDatWithPrimitiveValues()
        {
            // Arrange
            var expected = new[]
            {
                new object[] { "hello", 1, new RecordType<string>("world") },
                new object[] { "foo", 2, new RecordType<string>("bar") },
                new object[] { "Han", 3, new RecordType<string>("Solo") }
            };
            var sut = new ClassTestCaseSource(typeof(TestSourceWithMixedValues), Array.Empty<object>());
            var method = typeof(SampleTestType)
                .GetMethod(nameof(SampleTestType.TestMethodWithReferenceTypeParameter));

            // Act
            var actual = sut.GetTestCases(method).ToArray();

            // Assert
            Assert.Equal(expected, actual);
        }

        private class TestSourceWithMixedValues : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { "hello", 1, new RecordType<string>("world") };
                yield return new object[] { "foo", 2, new RecordType<string>("bar") };
                yield return new object[] { "Han", 3, new RecordType<string>("Solo") };
            }

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        }

        [Fact]
        public void ThrowsWhenConstructorParametersDontMatch()
        {
            // Arrange
            var parameters = new object[] { "a", 1 };
            var sut = new ClassTestCaseSource(typeof(TestSourceWithMixedValues), parameters);
            var method = typeof(SampleTestType)
                .GetMethod(nameof(SampleTestType.TestMethodWithReferenceTypeParameter));

            // Act & Assert
            Assert.Throws<MissingMethodException>(() => sut.GetTestCases(method).ToArray());
        }

        [Fact]
        public void AppliesExpectedConstructorParameters()
        {
            // Arrange
            object[] parameters = { new object[] { "y", 25 } };
            var sut = new ClassTestCaseSource(typeof(DelegatingTestData), parameters);
            var method = typeof(SampleTestType)
                .GetMethod(nameof(SampleTestType.TestMethodWithReferenceTypeParameter));

            // Act
            var result = sut.GetTestCases(method).ToArray();

            // Assert
            Assert.Equal(new object[] { "y", 25 }, result.Single());
        }
    }
}