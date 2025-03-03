using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.Kernel;
using AutoFixture.TUnit.UnitTest.TestTypes;
using TestTypeFoundation;

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
        public void IsDataAttribute()
        {
            // Arrange & Act
            var sut = new ClassAutoDataAttribute(typeof(MixedTypeClassData));

            // Assert
            Assert.That(sut).IsAssignableFrom<NonTypedDataSourceGeneratorAttribute>();
        }

        [Test]
        public void ThrowsWhenSourceTypeIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => new ClassAutoDataAttribute(null));
        }

        [Test]
        public void TreatsNullParameterValueAsArrayWithNull()
        {
            // Arrange & Act
            var sut = new ClassAutoDataAttribute(typeof(MixedTypeClassData), null);

            // Assert
            Assert.That(sut.Parameters).HasSingleItem()
                .And
                .IsNotNull();
        }

        [Test]
        public void ThrowsWhenFixtureFactoryIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => new DerivedClassAutoDataAttribute(
                    fixtureFactory: null, typeof(MixedTypeClassData)));
        }

        [Test]
        public async Task GetDataThrowsWhenSourceTypeNotEnumerable()
        {
            // Arrange
            var sut = new ClassAutoDataAttribute(typeof(MyClass));
            var testMethod = typeof(ExampleTestClass)
                .GetMethod(nameof(ExampleTestClass.TestMethod));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => sut.GetData(testMethod!, new DisposalTracker()).AsTask());
        }

        [Test]
        public async Task GetDataThrowsWhenParametersDoNotMatchConstructor()
        {
            // Arrange
            var sut = new ClassAutoDataAttribute(typeof(MyClass), "myString", 33, null);
            var testMethod = typeof(ExampleTestClass).GetMethod(nameof(ExampleTestClass.TestMethod));

            // Act & Assert
            await Assert.ThrowsAsync<MissingMethodException>(
                () => sut.GetData(testMethod!, new DisposalTracker()).AsTask());
        }

        [Test]
        public async Task GetDataDoesNotThrowWhenSourceYieldsNoResults()
        {
            // Arrange
            var sut = new ClassAutoDataAttribute(typeof(EmptyClassData));
            var testMethod = typeof(ExampleTestClass).GetMethod(nameof(ExampleTestClass.TestMethod));

            // Act
            var data = (await sut.GetData(testMethod!, new DisposalTracker()))
                .Select(x => x.GetData()).ToArray();

            // Assert
            Assert.Empty(data);
        }

        [Test]
        public async Task GetDataThrowsWhenSourceYieldsNullResults()
        {
            // Arrange
            var sut = new ClassAutoDataAttribute(typeof(ClassWithNullTestData));
            var testMethod = typeof(ExampleTestClass).GetMethod(nameof(ExampleTestClass.TestMethod));

            // Act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => sut.GetData(testMethod!, new DisposalTracker()).AsTask());
        }

        [Test]
        public async Task GetDataDoesNotThrow()
        {
            // Arrange
            var sut = new ClassAutoDataAttribute(typeof(MixedTypeClassData));
            var testMethod = typeof(ExampleTestClass).GetMethod(nameof(ExampleTestClass.TestMethod));

            // Act & Assert
            _ = await sut.GetData(testMethod!, new DisposalTracker());
        }

        [Test]
        public async Task GetDataReturnsEnumerable()
        {
            // Arrange
            var sut = new ClassAutoDataAttribute(typeof(MixedTypeClassData));
            var testMethod = typeof(ExampleTestClass).GetMethod(nameof(ExampleTestClass.TestMethod));

            // Act
            var actual = await sut.GetData(testMethod!, new DisposalTracker());

            // Assert
            Assert.NotNull(actual);
        }

        [Test]
        public async Task GetDataReturnsNonEmptyEnumerable()
        {
            // Arrange
            var sut = new ClassAutoDataAttribute(typeof(MixedTypeClassData));
            var testMethod = typeof(ExampleTestClass).GetMethod(nameof(ExampleTestClass.TestMethod));

            // Act
            var actual = await sut.GetData(testMethod!, new DisposalTracker());

            // Assert
            Assert.NotEmpty(actual);
        }

        [Test]
        public async Task GetDataReturnsExpectedTestDataCount()
        {
            // Arrange
            var sut = new ClassAutoDataAttribute(typeof(MixedTypeClassData));
            var testMethod = typeof(ExampleTestClass).GetMethod(nameof(ExampleTestClass.TestMethod));

            // Act
            var actual = await sut.GetData(testMethod!, new DisposalTracker());

            // Assert
            Assert.Equal(5, actual.Count);
        }

        [Test]
        public async Task GetDataThrowsWhenDataSourceNotEnumerable()
        {
            // Arrange
            var sut = new ClassAutoDataAttribute(typeof(GuardedConstructorHost<object>));
            var testMethod = typeof(ExampleTestClass).GetMethod(nameof(ExampleTestClass.TestMethod));

            // Act & Assert
            await Assert.ThrowsAsync<MissingMethodException>(
                () => sut.GetData(testMethod!, new DisposalTracker()).AsTask());
        }

        [Test]
        public async Task GetDataThrowsForNonMatchingConstructorTypes()
        {
            // Arrange
            var sut = new ClassAutoDataAttribute(typeof(DelegatingTestData), "myString", 33, null);
            var testMethod = typeof(ExampleTestClass).GetMethod(nameof(ExampleTestClass.TestMethod));

            // Act & Assert
            await Assert.ThrowsAsync<MissingMethodException>(
                () => sut.GetData(testMethod!, new DisposalTracker()).AsTask());
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
        public async Task GetDataOrdersCustomizationAttributes(string methodName)
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
            _ = await sut.GetData(method!, new DisposalTracker());

            // Assert
            var composite = Assert.IsAssignableFrom<CompositeCustomization>(customizationLog[0]);
            Assert.IsNotType<FreezeOnMatchCustomization>(composite.Customizations.First());
            Assert.IsType<FreezeOnMatchCustomization>(composite.Customizations.Last());
        }

        [Test]
        public async Task GetDataReturnsExpectedTestData()
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

            var actual = (await sut.GetData(testMethod!, new DisposalTracker()))
                .Select(x => x.GetData()).ToArray();

            Assert.Equal(expected, actual);
        }

        [Test]
        public async Task GetDataReturnsExpectedTestDataFromParameterizedSource()
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

            var actual = (await sut.GetData(testMethod!, new DisposalTracker()))
                .Select(x => x.GetData()).ToArray();

            Assert.Equal(expected, actual);
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
            var data = (await sut.GetData(testMethod!, new DisposalTracker()))
                .Select(x => x.GetData()).ToArray();

            // Assert
            Assert.Equal(expected, data);
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