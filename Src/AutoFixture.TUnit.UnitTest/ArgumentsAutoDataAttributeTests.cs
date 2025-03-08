using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoFixture.TUnit.UnitTest.TestTypes;
using TestTypeFoundation;
using TUnit.Core.Enums;

namespace AutoFixture.TUnit.UnitTest
{
    public class ArgumentsAutoDataAttributeTests
    {
        [Test]
        public async Task SutIsDataAttribute()
        {
            // Arrange & Act
            var sut = new ArgumentsAutoDataAttribute();

            // Assert
            await Assert.That(sut).IsAssignableTo<NonTypedDataSourceGeneratorAttribute>();
        }

        [Test]
        public async Task ValuesWillBeEmptyWhenSutIsCreatedWithDefaultConstructor()
        {
            // Arrange
            var sut = new ArgumentsAutoDataAttribute();
            var expected = Enumerable.Empty<object>();

            // Act
            var result = sut.Values;

            // Assert
            await Assert.That(result).IsEqualTo(expected);
        }

        [Test]
        public async Task ValuesWillNotBeEmptyWhenSutIsCreatedWithConstructorArguments()
        {
            // Arrange
            var expectedValues = new[] { new object(), new object(), new object() };
            var sut = new ArgumentsAutoDataAttribute(expectedValues);

            // Act
            var result = sut.Values;

            // Assert
            await Assert.That(result).IsEquivalentTo(expectedValues);
        }

        [Test]
        public async Task ValuesAreCorrectWhenConstructedWithExplicitAutoDataAttribute()
        {
            // Arrange
            var expectedValues = new[] { new object(), new object(), new object() };
            var sut = new DerivedArgumentsAutoDataAttribute(() => new DelegatingFixture(), expectedValues);

            // Act
            var result = sut.Values;

            // Assert
            await Assert.That(result).IsEqualTo(expectedValues);
        }

        [Test]
        public async Task DoesntActivateFixtureImmediately()
        {
            // Arrange
            var wasInvoked = false;

            // Act
            _ = new DerivedArgumentsAutoDataAttribute(() =>
            {
                wasInvoked = true;
                return new DelegatingFixture();
            });

            // Assert
            await Assert.That(wasInvoked).IsFalse();
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
            var customizationLog = new List<ICustomization>();
            var fixture = new DelegatingFixture
            {
                OnCustomize = c => customizationLog.Add(c)
            };
            var sut = new DerivedArgumentsAutoDataAttribute(() => fixture);

            // Act
            _ = sut.GenerateDataSources(DataGeneratorMetadataHelper.CreateDataGeneratorMetadata(typeof(TypeWithCustomizationAttributes), methodName));

            // Assert
            await Assert.That(customizationLog[0]).IsAssignableTo<CompositeCustomization>();

            var composite = (CompositeCustomization)customizationLog[0];

            await Assert.That(composite.Customizations.First()).IsNotTypeOf<FreezeOnMatchCustomization>();
            await Assert.That(composite.Customizations.Last()).IsAssignableTo<FreezeOnMatchCustomization>();
        }

        [Test]
        [MethodDataSource(typeof(InlinePrimitiveValuesTestData), nameof(InlinePrimitiveValuesTestData.GenerateDataSources))]
        [MethodDataSource(typeof(InlineFrozenValuesTestData), nameof(InlineFrozenValuesTestData.GenerateDataSources))]
        public async Task ReturnsSingleTestDataWithExpectedValues(NonTypedDataSourceGeneratorAttribute attribute, MethodInfo testMethod,
            object[] expected)
        {
            // Act
            var actual = attribute.GenerateDataSources(DataGeneratorMetadataHelper.CreateDataGeneratorMetadata(testMethod.DeclaringType, testMethod.Name)).ToArray();

            // Assert
            await Assert.That(actual).HasSingleItem();
            await Assert.That(actual[0]()).IsEqualTo(expected);
        }

        [Test]
        [ArgumentsAutoData]
        public async Task GeneratesRandomData(int a, float b, string c, decimal d)
        {
            await Assert.That(a).IsNotEqualTo(0);
            await Assert.That(b).IsNotEqualTo(0);
            await Assert.That(c).IsNotNull();
            await Assert.That(d).IsNotEqualTo(0);
        }

        [Test]
        [ArgumentsAutoData(12, 32.1f, "hello", 71.231d)]
        public async Task InlinesAllData(int a, float b, string c, decimal d)
        {
            await Assert.That(a).IsEqualTo(12);
            await Assert.That(b).IsEqualTo(32.1f);
            await Assert.That(c).IsEqualTo("hello");
            await Assert.That(d).IsEqualTo(71.231m);
        }

        [Test]
        [ArgumentsAutoData(0)]
        [ArgumentsAutoData(5)]
        [ArgumentsAutoData(-12)]
        [ArgumentsAutoData(21.3f)]
        [ArgumentsAutoData(18.7d)]
        [ArgumentsAutoData(EnumType.First)]
        [ArgumentsAutoData("Hello World")]
        [ArgumentsAutoData("\t\r\n")]
        [ArgumentsAutoData(" ")]
        [ArgumentsAutoData("")]
        [ArgumentsAutoData([null])]
        public async Task InjectsInlineValues([Frozen] object a,
            [Frozen] PropertyHolder<object> value,
            PropertyHolder<object> frozen)
        {
            await Assert.That(value.Property).IsEqualTo(a);
            await Assert.That(value).IsSameReferenceAs(frozen);
        }
    }
}