using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Kernel;
using AutoFixture.TUnit.UnitTest.TestTypes;
using TestTypeFoundation;
using TUnit.Assertions.AssertConditions.Throws;

namespace AutoFixture.TUnit.UnitTest
{
    public class ClassAutoDataAttributeTests
    {
        [Test]
        public void CanCreateInstance()
        {
            // Act & Assert
            _ = new ClassAutoDataAttribute(typeof(MixedTypeClassData));
        }

        [Test]
        public async Task IsDataAttribute()
        {
            // Arrange & Act
            var sut = new ClassAutoDataAttribute(typeof(MixedTypeClassData));

            // Assert
            await Assert.That(sut).IsAssignableTo<NonTypedDataSourceGeneratorAttribute>();
        }

        [Test]
        public async Task ThrowsWhenSourceTypeIsNull()
        {
            // Act & Assert
            await Assert.That(() => new ClassAutoDataAttribute(null)).ThrowsExactly<ArgumentNullException>();
        }

        [Test]
        public async Task TreatsNullParameterValueAsArrayWithNull()
        {
            // Arrange & Act
            var sut = new ClassAutoDataAttribute(typeof(MixedTypeClassData), null);

            // Assert
            await Assert.That(sut.Parameters).HasSingleItem()
                .And
                .IsNotNull();
        }

        [Test]
        public async Task ThrowsWhenFixtureFactoryIsNull()
        {
            // Act & Assert
            await Assert.That(() => new DerivedClassAutoDataAttribute(
                    fixtureFactory: null, typeof(MixedTypeClassData))).ThrowsExactly<ArgumentNullException>();
        }

        [Test]
        public async Task GenerateDataSourcesThrowsWhenSourceTypeNotEnumerable()
        {
            // Arrange
            var sut = new ClassAutoDataAttribute(typeof(MyClass));
            var testMethod = typeof(ExampleTestClass)
                .GetMethod(nameof(ExampleTestClass.TestMethod));

            // Act & Assert
            await Assert.That(() => sut.GenerateDataSources(DataGeneratorMetadataHelper.CreateDataGeneratorMetadata(testMethod!))).ThrowsExactly<InvalidOperationException>();
        }

        [Test]
        public async Task GenerateDataSourcesThrowsWhenParametersDoNotMatchConstructor()
        {
            // Arrange
            var sut = new ClassAutoDataAttribute(typeof(MyClass), "myString", 33, null);
            var testMethod = typeof(ExampleTestClass).GetMethod(nameof(ExampleTestClass.TestMethod));

            // Act & Assert
            await Assert.That(() => sut.GenerateDataSources(DataGeneratorMetadataHelper.CreateDataGeneratorMetadata(testMethod!))).ThrowsExactly<MissingMethodException>();
        }

        [Test]
        public async Task GenerateDataSourcesDoesNotThrowWhenSourceYieldsNoResults()
        {
            // Arrange
            var sut = new ClassAutoDataAttribute(typeof(EmptyClassData));
            var testMethod = typeof(ExampleTestClass).GetMethod(nameof(ExampleTestClass.TestMethod));

            // Act
            var data = sut.GenerateDataSources(DataGeneratorMetadataHelper.CreateDataGeneratorMetadata(testMethod!))
                .Select(x => x());

            // Assert
            await Assert.That(data).IsEmpty();
        }

        [Test]
        public async Task GenerateDataSourcesThrowsWhenSourceYieldsNullResults()
        {
            // Arrange
            var sut = new ClassAutoDataAttribute(typeof(ClassWithNullTestData));
            var testMethod = typeof(ExampleTestClass).GetMethod(nameof(ExampleTestClass.TestMethod));

            // Act & assert
            await Assert.That(() => sut.GenerateDataSources(DataGeneratorMetadataHelper.CreateDataGeneratorMetadata(testMethod!))).ThrowsExactly<InvalidOperationException>();
        }

        [Test]
        public async Task GenerateDataSourcesDoesNotThrow()
        {
            // Arrange
            var sut = new ClassAutoDataAttribute(typeof(MixedTypeClassData));
            var testMethod = typeof(ExampleTestClass).GetMethod(nameof(ExampleTestClass.TestMethod));

            // Act & Assert
            sut.GenerateDataSources(DataGeneratorMetadataHelper.CreateDataGeneratorMetadata(testMethod!));
        }

        [Test]
        public async Task GenerateDataSourcesReturnsEnumerable()
        {
            // Arrange
            var sut = new ClassAutoDataAttribute(typeof(MixedTypeClassData));
            var testMethod = typeof(ExampleTestClass).GetMethod(nameof(ExampleTestClass.TestMethod));

            // Act
            var actual = sut.GenerateDataSources(DataGeneratorMetadataHelper.CreateDataGeneratorMetadata(testMethod!));

            // Assert
            await Assert.That(actual).IsNotNull();
        }

        [Test]
        public async Task GenerateDataSourcesReturnsNonEmptyEnumerable()
        {
            // Arrange
            var sut = new ClassAutoDataAttribute(typeof(MixedTypeClassData));
            var testMethod = typeof(ExampleTestClass).GetMethod(nameof(ExampleTestClass.TestMethod));

            // Act
            var actual = sut.GenerateDataSources(DataGeneratorMetadataHelper.CreateDataGeneratorMetadata(testMethod!));

            // Assert
            await Assert.That(actual).IsNotEmpty();
        }

        [Test]
        public async Task GenerateDataSourcesReturnsExpectedTestDataCount()
        {
            // Arrange
            var sut = new ClassAutoDataAttribute(typeof(MixedTypeClassData));
            var testMethod = typeof(ExampleTestClass).GetMethod(nameof(ExampleTestClass.TestMethod));

            // Act
            var actual = sut.GenerateDataSources(DataGeneratorMetadataHelper.CreateDataGeneratorMetadata(testMethod!));

            // Assert
            await Assert.That(actual).HasCount().EqualTo(5);
        }

        [Test]
        public async Task GenerateDataSourcesThrowsWhenDataSourceNotEnumerable()
        {
            // Arrange
            var sut = new ClassAutoDataAttribute(typeof(GuardedConstructorHost<object>));
            var testMethod = typeof(ExampleTestClass).GetMethod(nameof(ExampleTestClass.TestMethod));

            // Act & Assert
            await Assert.That(() => sut.GenerateDataSources(DataGeneratorMetadataHelper.CreateDataGeneratorMetadata(testMethod!))).ThrowsExactly<MissingMethodException>();
        }

        [Test]
        public async Task GenerateDataSourcesThrowsForNonMatchingConstructorTypes()
        {
            // Arrange
            var sut = new ClassAutoDataAttribute(typeof(DelegatingTestData), "myString", 33, null);
            var testMethod = typeof(ExampleTestClass).GetMethod(nameof(ExampleTestClass.TestMethod));

            // Act & Assert
            await Assert.That(() => sut.GenerateDataSources(DataGeneratorMetadataHelper.CreateDataGeneratorMetadata(testMethod!))).ThrowsExactly<MissingMethodException>();
        }

        [Test]
        [Arguments("CreateWithFrozenAndFavorArrays")]
        [Arguments("CreateWithFavorArraysAndFrozen")]
        [Arguments("CreateWithFrozenAndFavorEnumerables")]
        [Arguments("CreateWithFavorEnumerablesAndFrozen")]
        [Arguments("CreateWithFrozenAndFavorLists")]
        [Arguments("CreateWithFavorListsAndFrozen")]
        [Arguments("CreateWithFrozenAndGreedy")]
        [Arguments("CreateWithGreedyAndFrozen")]
        [Arguments("CreateWithFrozenAndModest")]
        [Arguments("CreateWithModestAndFrozen")]
        [Arguments("CreateWithFrozenAndNoAutoProperties")]
        [Arguments("CreateWithNoAutoPropertiesAndFrozen")]
        public async Task GenerateDataSourcesOrdersCustomizationAttributes(string methodName)
        {
            // Arrange
            var method = typeof(TypeWithCustomizationAttributes)
                .GetMethod(methodName, [typeof(ConcreteType)]);
            var customizationLog = new List<ICustomization>();
            var fixture = new DelegatingFixture
            {
                OnCustomize = c => customizationLog.Add(c)
            };

            var sut = new DerivedClassAutoDataAttribute(() => fixture, typeof(ClassWithEmptyTestData));

            // Act
            _ = sut.GenerateDataSources(DataGeneratorMetadataHelper.CreateDataGeneratorMetadata(method!));

            // Assert
            await Assert.That(customizationLog[0]).IsAssignableTo<CompositeCustomization>();

            var composite = (CompositeCustomization)customizationLog[0];

            await Assert.That(composite.Customizations.First()).IsNotTypeOf<FreezeOnMatchCustomization>();
            await Assert.That(composite.Customizations.Last()).IsAssignableTo<FreezeOnMatchCustomization>();
        }

        [Test]
        public async Task GenerateDataSourcesReturnsExpectedTestData()
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
            [
                [1, "value", EnumType.First, new Tuple<string, int>("value", 1)],
                [9, "value", EnumType.First, new Tuple<string, int>("value", 1)],
                [12, "test-12", EnumType.First, new Tuple<string, int>("value", 1)],
                [223, "test-17", EnumType.Third, new Tuple<string, int>("value", 1)],
                [-95, "test-92", EnumType.Second, new Tuple<string, int>("myValue", 5)]
            ];

            var actual = sut.GenerateDataSources(DataGeneratorMetadataHelper.CreateDataGeneratorMetadata(testMethod!))
                .Select(x => x())
                .ToArray();

            await Assert.That(actual).IsEquivalentTo(expected);
        }

        [Test]
        public async Task GenerateDataSourcesReturnsExpectedTestDataFromParameterizedSource()
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
            [
                [29, "myValue", EnumType.Third, new Tuple<string, int>("value", 1)],
                [29, "myValue", EnumType.Third, new Tuple<string, int>("value", 1)]
            ];

            var actual = sut.GenerateDataSources(DataGeneratorMetadataHelper.CreateDataGeneratorMetadata(testMethod!))
                .Select(x => x())
                .ToArray();

            await Assert.That(actual).IsEquivalentTo(expected);
        }

        [Test]
        public async Task TestWithNullParametersPasses()
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
            var actual = sut.GenerateDataSources(DataGeneratorMetadataHelper.CreateDataGeneratorMetadata(testMethod!))
                .Select(x => x())
                .ToArray();

            // Assert
            await Assert.That(actual).IsEquivalentTo(expected);
        }

        public class TestDataWithNullValues : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return [null, null, null, null];
                yield return [string.Empty, null, null, null];
                yield return [null, "  ", null, null];
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }
    }
}