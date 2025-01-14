using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Kernel;
using AutoFixture.Xunit2.UnitTest.TestTypes;
using TestTypeFoundation;
using Xunit;
using Xunit.Sdk;

namespace AutoFixture.Xunit2.UnitTest
{
    public class ClassAutoDataAttributeTests
    {
        [Fact]
        public void CanCreateInstance()
        {
            // Act & Assert
            _ = new ClassAutoDataAttribute(typeof(MixedTypeClassData));
        }

        [Fact]
        public void IsDataAttribute()
        {
            // Arrange & Act
            var sut = new ClassAutoDataAttribute(typeof(MixedTypeClassData));

            // Assert
            Assert.IsAssignableFrom<DataAttribute>(sut);
        }

        [Fact]
        public void ThrowsWhenSourceTypeIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ClassAutoDataAttribute(null));
        }

        [Fact]
        public void ThrowsWhenParametersIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ClassAutoDataAttribute(typeof(MixedTypeClassData), null));
        }

        [Fact]
        public void ThrowsWhenFixtureFactoryIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => new DerivedClassAutoDataAttribute(null, typeof(MixedTypeClassData)));
        }

        [Fact]
        public void GetDataThrowsWhenSourceTypeNotEnumerable()
        {
            // Arrange
            var sut = new ClassAutoDataAttribute(typeof(MyClass));
            var testMethod = typeof(ExampleTestClass).GetMethod(nameof(ExampleTestClass.TestMethod));

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _ = sut.GetData(testMethod).ToArray());
        }

        [Fact]
        public void GetDataThrowsWhenParametersDoNotMatchConstructor()
        {
            // Arrange
            var sut = new ClassAutoDataAttribute(typeof(MyClass), "myString", 33, null);
            var testMethod = typeof(ExampleTestClass).GetMethod(nameof(ExampleTestClass.TestMethod));

            // Act & Assert
            Assert.Throws<MissingMethodException>(() => _ = sut.GetData(testMethod).ToArray());
        }

        [Fact]
        public void GetDataDoesNotThrowWhenSourceYieldsNoResults()
        {
            // Arrange
            var sut = new ClassAutoDataAttribute(typeof(EmptyClassData));
            var testMethod = typeof(ExampleTestClass).GetMethod(nameof(ExampleTestClass.TestMethod));

            // Act
            var data = sut.GetData(testMethod).ToArray();

            // Assert
            Assert.Empty(data);
        }

        [Fact]
        public void GetDataThrowsWhenSourceYieldsNullResults()
        {
            // Arrange
            var sut = new ClassAutoDataAttribute(typeof(ClassWithNullTestData));
            var testMethod = typeof(ExampleTestClass).GetMethod(nameof(ExampleTestClass.TestMethod));

            // Act & assert
            Assert.Throws<InvalidOperationException>(() => sut.GetData(testMethod).ToArray());
        }

        [Fact]
        public void GetDataDoesNotThrow()
        {
            // Arrange
            var sut = new ClassAutoDataAttribute(typeof(MixedTypeClassData));
            var testMethod = typeof(ExampleTestClass).GetMethod(nameof(ExampleTestClass.TestMethod));

            // Act & Assert
            _ = sut.GetData(testMethod);
        }

        [Fact]
        public void GetDataReturnsEnumerable()
        {
            // Arrange
            var sut = new ClassAutoDataAttribute(typeof(MixedTypeClassData));
            var testMethod = typeof(ExampleTestClass).GetMethod(nameof(ExampleTestClass.TestMethod));

            // Act
            var actual = sut.GetData(testMethod);

            // Assert
            Assert.NotNull(actual);
        }

        [Fact]
        public void GetDataReturnsNonEmptyEnumerable()
        {
            // Arrange
            var sut = new ClassAutoDataAttribute(typeof(MixedTypeClassData));
            var testMethod = typeof(ExampleTestClass).GetMethod(nameof(ExampleTestClass.TestMethod));

            // Act
            var actual = sut.GetData(testMethod);

            // Assert
            Assert.NotEmpty(actual);
        }

        [Fact]
        public void GetDataReturnsExpectedTestDataCount()
        {
            // Arrange
            var sut = new ClassAutoDataAttribute(typeof(MixedTypeClassData));
            var testMethod = typeof(ExampleTestClass).GetMethod(nameof(ExampleTestClass.TestMethod));

            // Act
            var actual = sut.GetData(testMethod);

            // Assert
            Assert.Equal(5, actual.Count());
        }

        [Fact]
        public void GetDataThrowsWhenDataSourceNotEnumerable()
        {
            // Arrange
            var sut = new ClassAutoDataAttribute(typeof(GuardedConstructorHost<object>));
            var testMethod = typeof(ExampleTestClass).GetMethod(nameof(ExampleTestClass.TestMethod));

            // Act & Assert
            Assert.Throws<MissingMethodException>(() => _ = sut.GetData(testMethod).ToArray());
        }

        [Fact]
        public void GetDataThrowsForNonMatchingConstructorTypes()
        {
            // Arrange
            var sut = new ClassAutoDataAttribute(typeof(DelegatingTestData), "myString", 33, null);
            var testMethod = typeof(ExampleTestClass).GetMethod(nameof(ExampleTestClass.TestMethod));

            // Act & Assert
            Assert.Throws<MissingMethodException>(() => _ = sut.GetData(testMethod).ToArray());
        }

        [Theory]
        [InlineData("CreateWithFrozenAndFavorArrays")]
        [InlineData("CreateWithFavorArraysAndFrozen")]
        [InlineData("CreateWithFrozenAndFavorEnumerables")]
        [InlineData("CreateWithFavorEnumerablesAndFrozen")]
        [InlineData("CreateWithFrozenAndFavorLists")]
        [InlineData("CreateWithFavorListsAndFrozen")]
        [InlineData("CreateWithFrozenAndGreedy")]
        [InlineData("CreateWithGreedyAndFrozen")]
        [InlineData("CreateWithFrozenAndModest")]
        [InlineData("CreateWithModestAndFrozen")]
        [InlineData("CreateWithFrozenAndNoAutoProperties")]
        [InlineData("CreateWithNoAutoPropertiesAndFrozen")]
        public void GetDataOrdersCustomizationAttributes(string methodName)
        {
            // Arrange
            var method = typeof(TypeWithCustomizationAttributes)
                .GetMethod(methodName, new[] { typeof(ConcreteType) });
            var customizationLog = new List<ICustomization>();
            var fixture = new DelegatingFixture
            {
                OnCustomize = c => customizationLog.Add(c)
            };

            var sut = new DerivedClassAutoDataAttribute(() => fixture, typeof(ClassWithEmptyTestData));

            // Act
            _ = sut.GetData(method).ToArray();

            // Assert
            var composite = Assert.IsAssignableFrom<CompositeCustomization>(customizationLog[0]);
            Assert.IsNotType<FreezeOnMatchCustomization>(composite.Customizations.First());
            Assert.IsType<FreezeOnMatchCustomization>(composite.Customizations.Last());
        }

        [Fact]
        public void GetDataReturnsExpectedTestData()
        {
            var builder = new CompositeSpecimenBuilder(
                new FixedParameterBuilder<int>("a", 1),
                new FixedParameterBuilder<string>("b", "value"),
                new FixedParameterBuilder<EnumType>("c", EnumType.First),
                new FixedParameterBuilder<Tuple<string, int>>("d", new Tuple<string, int>("value", 1)));
            var sut = new DerivedClassAutoDataAttribute(
                () => new DelegatingFixture { OnCreate = (r, c) => builder.Create(r, c) },
                typeof(MixedTypeClassData));
            var testMethod = typeof(ExampleTestClass).GetMethod(nameof(ExampleTestClass.TestMethod));
            object[][] expected =
            {
                new object[] { 1, "value", EnumType.First, new Tuple<string, int>("value", 1) },
                new object[] { 9, "value", EnumType.First, new Tuple<string, int>("value", 1) },
                new object[] { 12, "test-12", EnumType.First, new Tuple<string, int>("value", 1) },
                new object[] { 223, "test-17", EnumType.Third, new Tuple<string, int>("value", 1) },
                new object[] { -95, "test-92", EnumType.Second, new Tuple<string, int>("myValue", 5) }
            };

            var actual = sut.GetData(testMethod).ToArray();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetDataReturnsExpectedTestDataFromParameterizedSource()
        {
            var builder = new CompositeSpecimenBuilder(
                new FixedParameterBuilder<int>("a", 1),
                new FixedParameterBuilder<string>("b", "value"),
                new FixedParameterBuilder<Tuple<string, int>>("d", new Tuple<string, int>("value", 1)));
            var sut = new DerivedClassAutoDataAttribute(
                () => new DelegatingFixture { OnCreate = (r, c) => builder.Create(r, c) },
                typeof(ParameterizedClassData),
                29, "myValue", EnumType.Third);
            var testMethod = typeof(ExampleTestClass).GetMethod(nameof(ExampleTestClass.TestMethod));
            object[][] expected =
            {
                new object[] { 29, "myValue", EnumType.Third, new Tuple<string, int>("value", 1) },
                new object[] { 29, "myValue", EnumType.Third, new Tuple<string, int>("value", 1) }
            };

            var actual = sut.GetData(testMethod).ToArray();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestWithNullParametersPasses()
        {
            // Arrange
            var sut = new ClassAutoDataAttribute(typeof(TestDataWithNullValues));
            var testMethod = typeof(ExampleTestClass<string, string, string[], RecordType<string>>)
                .GetMethod(nameof(ExampleTestClass<string, string, string[], RecordType<string>>.TestMethod));
            var expected = new[]
            {
                new object[] { null, null, null, null },
                new object[] { string.Empty, null, null, null },
                new object[] { null, "  ", null, null },
            };

            // Act
            var data = sut.GetData(testMethod).ToArray();

            // Assert
            Assert.Equal(expected, data);
        }

        public class TestDataWithNullValues : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { null, null, null, null };
                yield return new object[] { string.Empty, null, null, null };
                yield return new object[] { null, "  ", null, null };
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }
    }
}