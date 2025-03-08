using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.TUnit.Internal;
using AutoFixture.TUnit.UnitTest.TestTypes;
using TestTypeFoundation;
using TUnit.Assertions.AssertConditions.Throws;

namespace AutoFixture.TUnit.UnitTest.Internal
{
    public class MethodDataSourceTests
    {
        public static IEnumerable<object[]> GetTestDataFieldWithMixedValues()
        {
            yield return ["hello", 1, new RecordType<string>("world")];
            yield return ["foo", 2, new RecordType<string>("bar")];
            yield return ["Han", 3, new RecordType<string>("Solo")];
        }

        [Test]
        public async Task SutIsTestDataSource()
        {
            // Arrange
            var methodInfo = typeof(MethodDataSourceTests)
                .GetMethod(nameof(this.SutIsTestDataSource));

            // Act
            var sut = new MethodDataSource(methodInfo);

            // Assert
            await Assert.That(sut).IsTypeOf<DataSource>();
        }

        [Test]
        public async Task ThrowsWhenMethodInfoIsNull()
        {
            // Act & Assert
            await Assert.That(() =>
                new MethodDataSource(null!)).ThrowsExactly<ArgumentNullException>();
        }

        [Test]
        public async Task ThrowsWhenArgumentsIsNull()
        {
            // Arrange
            var methodInfo = typeof(MethodDataSourceTests)
                .GetMethod(nameof(this.SutIsTestDataSource));

            // Act & Assert
            await Assert.That(() =>
                new MethodDataSource(methodInfo, null!)).ThrowsExactly<ArgumentNullException>();
        }

        [Test]
        public async Task ConstructorSetsProperties()
        {
            // Arrange
            var methodInfo = typeof(MethodDataSourceTests)
                .GetMethod(nameof(this.SutIsTestDataSource));
            var arguments = new[] { new object() };

            // Act
            var sut = new MethodDataSource(methodInfo, arguments);

            // Assert
            await Assert.That(sut.MethodInfo).IsEqualTo(methodInfo);
            await Assert.That(sut.Arguments).IsEqualTo(arguments);
        }

        [Test]
        public async Task GetTestDataInvokesMethodInfo()
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
            var result = sut.GenerateDataSources(testData);

            // Assert
            await Assert.That(result).IsEqualTo(expected);
        }

        [Test]
        public async Task ThrowsWhenMemberDoesNotReturnAnEnumerableValue()
        {
            // Arrange
            var dataSource = typeof(MethodDataSourceTests)
                .GetMethod(nameof(NonEnumerableTestData));
            var testData = typeof(SampleTestType)
                .GetMethod(nameof(SampleTestType.TestMethodWithReferenceTypeParameter));
            var sut = new MethodDataSource(dataSource);

            // Act & Assert
            await Assert.That(() => sut.GenerateDataSources(testData).ToArray()).ThrowsExactly<InvalidCastException>();
        }

        public static object NonEnumerableTestData() => new();
    }
}