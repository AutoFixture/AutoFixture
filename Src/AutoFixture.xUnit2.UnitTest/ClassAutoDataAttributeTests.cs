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
            _ = new ClassAutoDataAttribute(typeof(MixedTypeClassData));
        }

        [Fact]
        public void IsDataAttribute()
        {
            var sut = new ClassAutoDataAttribute(typeof(MixedTypeClassData));

            Assert.IsAssignableFrom<DataAttribute>(sut);
        }

        [Fact]
        public void ThrowsWhenSourceTypeIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new ClassAutoDataAttribute(null));
        }

        [Fact]
        public void ThrowsWhenParametersIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new ClassAutoDataAttribute(typeof(MixedTypeClassData), null));
        }

        [Fact]
        public void ThrowsWhenFixtureFactoryIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new DerivedClassAutoDataAttribute(null, typeof(MixedTypeClassData)));
        }

        [Fact]
        public void GetDataThrowsWhenSourceTypeReturnsNull()
        {
            var sut = new ClassAutoDataAttribute(typeof(NullClassData));
            var testMethod = typeof(ExampleTestClass).GetMethod(nameof(ExampleTestClass.TestMethod));

            Action act = () => _ = sut.GetData(testMethod).ToArray();

            Assert.Throws<InvalidOperationException>(act);
        }

        [Fact]
        public void GetDataThrowsWhenSourceTypeNotEnumerable()
        {
            var sut = new ClassAutoDataAttribute(typeof(MyClass));
            var testMethod = typeof(ExampleTestClass).GetMethod(nameof(ExampleTestClass.TestMethod));

            Action act = () => _ = sut.GetData(testMethod).ToArray();

            Assert.Throws<InvalidOperationException>(act);
        }

        [Fact]
        public void GetDataThrowsWhenParametersDoNotMatchConstructor()
        {
            var sut = new ClassAutoDataAttribute(typeof(MyClass), "myString", 33, null);
            var testMethod = typeof(ExampleTestClass).GetMethod(nameof(ExampleTestClass.TestMethod));

            Action act = () => _ = sut.GetData(testMethod).ToArray();

            Assert.Throws<MissingMethodException>(act);
        }

        [Fact]
        public void GetDataDoesNotThrowWhenSourceYieldsNoResults()
        {
            var sut = new ClassAutoDataAttribute(typeof(EmptyClassData));
            var testMethod = typeof(ExampleTestClass).GetMethod(nameof(ExampleTestClass.TestMethod));

            var data = sut.GetData(testMethod).ToArray();

            Assert.Empty(data);
        }

        [Fact]
        public void GetDataThrowsWhenSourceYieldsNullResults()
        {
            var sut = new ClassAutoDataAttribute(typeof(ClassWithNullTestCases));
            var testMethod = typeof(ExampleTestClass).GetMethod(nameof(ExampleTestClass.TestMethod));

            var data = sut.GetData(testMethod).ToArray();

            Assert.Equal(3, data.Length);
        }

        [Fact]
        public void GetDataDoesNotThrow()
        {
            var sut = new ClassAutoDataAttribute(typeof(MixedTypeClassData));
            var testMethod = typeof(ExampleTestClass).GetMethod(nameof(ExampleTestClass.TestMethod));

            _ = sut.GetData(testMethod);
        }

        [Fact]
        public void GetDataReturnsEnumerable()
        {
            var sut = new ClassAutoDataAttribute(typeof(MixedTypeClassData));
            var testMethod = typeof(ExampleTestClass).GetMethod(nameof(ExampleTestClass.TestMethod));

            var actual = sut.GetData(testMethod);

            Assert.NotNull(actual);
        }

        [Fact]
        public void GetDataReturnsNonEmptyEnumerable()
        {
            var sut = new ClassAutoDataAttribute(typeof(MixedTypeClassData));
            var testMethod = typeof(ExampleTestClass).GetMethod(nameof(ExampleTestClass.TestMethod));

            var actual = sut.GetData(testMethod);

            Assert.NotEmpty(actual);
        }

        [Fact]
        public void GetDataReturnsExpectedTestCaseCount()
        {
            var sut = new ClassAutoDataAttribute(typeof(MixedTypeClassData));
            var testMethod = typeof(ExampleTestClass).GetMethod(nameof(ExampleTestClass.TestMethod));

            var actual = sut.GetData(testMethod);

            Assert.Equal(5, actual.Count());
        }

        [Fact]
        public void GetDataThrowsWhenDataSourceNotEnumerable()
        {
            var sut = new ClassAutoDataAttribute(typeof(GuardedConstructorHost<object>));
            var testMethod = typeof(ExampleTestClass).GetMethod(nameof(ExampleTestClass.TestMethod));

            Action act = () => _ = sut.GetData(testMethod).ToArray();

            Assert.Throws<MissingMethodException>(act);
        }

        [Fact]
        public void GetDataThrowsForNonMatchingConstructorTypes()
        {
            var sut = new ClassAutoDataAttribute(
                typeof(DelegatingTestData), "myString", 33, null);
            var testMethod = typeof(ExampleTestClass).GetMethod(nameof(ExampleTestClass.TestMethod));

            Action act = () => _ = sut.GetData(testMethod).ToArray();

            Assert.Throws<MissingMethodException>(act);
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
            var fixture = new DelegatingFixture();
            fixture.OnCustomize = c =>
            {
                customizationLog.Add(c);
                return fixture;
            };

            var sut = new DerivedClassAutoDataAttribute(() => fixture, typeof(ClassWithEmptyTestCases));

            // Act
            var data = sut.GetData(method).ToArray();

            // Assert
            var composite = Assert.IsAssignableFrom<CompositeCustomization>(customizationLog[0]);
            Assert.IsNotType<FreezeOnMatchCustomization>(composite.Customizations.First());
            Assert.IsType<FreezeOnMatchCustomization>(composite.Customizations.Last());
        }

        [Fact]
        public void GetDataReturnsExpectedTestCases()
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
        public void GetDataReturnsExpectedTestCasesFromParameterizedSource()
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
    }
}
