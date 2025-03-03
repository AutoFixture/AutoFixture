using System;
using System.Collections;
using System.Collections.Generic;
using AutoFixture.TUnit.UnitTest.TestTypes;
using TestTypeFoundation;

namespace AutoFixture.TUnit.UnitTest.Internal
{
    public class ClassDataSourceTests
    {
        [Test]
        public void SutIsTestDataSource()
        {
            // Arrange & Act
            var sut = new ClassDataSource(typeof(object), Array.Empty<object>());

            // Assert
            Assert.IsAssignableFrom<IDataSource>(sut);
        }

        [Test]
        public void ConstructorWithNullTypeThrows()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _ = new ClassDataSource(null!, Array.Empty<object>()));
        }

        [Test]
        public void ConstructorWithNullParametersThrows()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _ = new ClassDataSource(typeof(object), null!));
        }

        [Test]
        public void TypeIsCorrect()
        {
            // Arrange
            var expected = typeof(object);
            var sut = new ClassDataSource(expected, Array.Empty<object>());

            // Act
            var result = sut.Type;

            // Assert
            Assert.That(result).IsEqualTo(expected);
        }

        [Test]
        public void ParametersIsCorrect()
        {
            // Arrange
            var expected = new[] { new object() };
            var sut = new ClassDataSource(typeof(object), expected);

            // Act
            var result = sut.Parameters;

            // Assert
            Assert.That(result).IsEqualTo(expected);
        }

        [Test]
        public void ThrowsWhenSourceIsNotEnumerable()
        {
            // Arrange
            var sut = new ClassDataSource(typeof(object), Array.Empty<object>());
            var method = typeof(SampleTestType)
                .GetMethod(nameof(SampleTestType.TestMethodWithReferenceTypeParameter));

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => sut.GetData(method).ToArray());
        }

        [Test]
        public void GeneratesTestDatWithPrimitiveValues()
        {
            // Arrange
            var expected = new[]
            {
                new object[] { "hello", 1, new RecordType<string>("world") },
                new object[] { "foo", 2, new RecordType<string>("bar") },
                new object[] { "Han", 3, new RecordType<string>("Solo") }
            };
            var sut = new ClassDataSource(typeof(TestSourceWithMixedValues), Array.Empty<object>());
            var method = typeof(SampleTestType)
                .GetMethod(nameof(SampleTestType.TestMethodWithReferenceTypeParameter));

            // Act
            var actual = sut.GetData(method).ToArray();

            // Assert
            Assert.Equal(expected, actual);
        }

        private class TestSourceWithMixedValues : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return ["hello", 1, new RecordType<string>("world")];
                yield return ["foo", 2, new RecordType<string>("bar")];
                yield return ["Han", 3, new RecordType<string>("Solo")];
            }

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        }

        [Test]
        public void ThrowsWhenConstructorParametersDontMatch()
        {
            // Arrange
            var parameters = new object[] { "a", 1 };
            var sut = new ClassDataSource(typeof(TestSourceWithMixedValues), parameters);
            var method = typeof(SampleTestType)
                .GetMethod(nameof(SampleTestType.TestMethodWithReferenceTypeParameter));

            // Act & Assert
            Assert.Throws<MissingMethodException>(() => sut.GetData(method).ToArray());
        }

        [Test]
        public void AppliesExpectedConstructorParameters()
        {
            // Arrange
            object[] parameters = [new object[] { "y", 25 }];
            var sut = new ClassDataSource(typeof(DelegatingTestData), parameters);
            var method = typeof(SampleTestType)
                .GetMethod(nameof(SampleTestType.TestMethodWithReferenceTypeParameter));

            // Act
            var result = sut.GetData(method).ToArray();

            // Assert
            Assert.Equal(new object[] { "y", 25 }, result.Single());
        }
    }
}