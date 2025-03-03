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
        public void SutIsDataAttribute()
        {
            // Arrange & Act
            var sut = new ArgumentsAutoDataAttribute();

            // Assert
            Assert.That(sut).IsAssignableFrom<NonTypedDataSourceGeneratorAttribute>();
        }

        [Test]
        public void ValuesWillBeEmptyWhenSutIsCreatedWithDefaultConstructor()
        {
            // Arrange
            var sut = new ArgumentsAutoDataAttribute();
            var expected = Enumerable.Empty<object>();

            // Act
            var result = sut.Values;

            // Assert
            Assert.That(result).IsEqualTo(expected);
        }

        [Test]
        public void ValuesWillNotBeEmptyWhenSutIsCreatedWithConstructorArguments()
        {
            // Arrange
            var expectedValues = new[] { new object(), new object(), new object() };
            var sut = new ArgumentsAutoDataAttribute(expectedValues);

            // Act
            var result = sut.Values;

            // Assert
            Assert.That(result).IsEquivalentTo(expectedValues);
        }

        [Test]
        public void ValuesAreCorrectWhenConstructedWithExplicitAutoDataAttribute()
        {
            // Arrange
            var expectedValues = new[] { new object(), new object(), new object() };
            var sut = new DerivedArgumentsAutoDataAttribute(() => new DelegatingFixture(), expectedValues);

            // Act
            var result = sut.Values;

            // Assert
            Assert.That(result).IsEqualTo(expectedValues);
        }

        [Test]
        public void DoesntActivateFixtureImmediately()
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
            Assert.That(wasInvoked).IsFalse();
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
            var customizationLog = new List<ICustomization>();
            var fixture = new DelegatingFixture
            {
                OnCustomize = c => customizationLog.Add(c)
            };
            var sut = new DerivedArgumentsAutoDataAttribute(() => fixture);

            // Act
            _ = sut.GenerateDataSources(DataGeneratorMetadataHelper.CreateDataGeneratorMetadata(typeof(TypeWithCustomizationAttributes), methodName));

            // Assert
            Assert.That(customizationLog[0]).IsAssignableFrom<CompositeCustomization>();
            
            var composite = (CompositeCustomization) customizationLog[0];
            
            Assert.That(composite.Customizations.First()).IsNotTypeOf<FreezeOnMatchCustomization>();
            Assert.That(composite.Customizations.Last()).IsTypeOf<FreezeOnMatchCustomization>();
        }

        [Test]
        [MethodDataSource(typeof(InlinePrimitiveValuesTestData), nameof(InlinePrimitiveValuesTestData.GetEnumerator))]
        [MethodDataSource(typeof(InlineFrozenValuesTestData), nameof(InlineFrozenValuesTestData.GetEnumerator))]
        public async Task ReturnsSingleTestDataWithExpectedValues(NonTypedDataSourceGeneratorAttribute attribute, MethodInfo testMethod,
            object[] expected)
        {
            // Act
            var actual = attribute.GenerateDataSources(DataGeneratorMetadataHelper.CreateDataGeneratorMetadata(testMethod.DeclaringType, testMethod.Name)).ToArray();

            // Assert
            Assert.That(actual).HasSingleItem();
            Assert.That(actual[0]()).IsEqualTo(expected);
        }

        [Test]
        [ArgumentsAutoData]
        public void GeneratesRandomData(int a, float b, string c, decimal d)
        {
            Assert.That(a).IsNotEqualTo(0);
            Assert.That(b).IsNotEqualTo(0);
            Assert.That(c).IsNotNull();
            Assert.That(d).IsNotEqualTo(0);
        }

        [Test]
        [ArgumentsAutoData(12, 32.1f, "hello", 71.231d)]
        public void InlinesAllData(int a, float b, string c, decimal d)
        {
            Assert.That(a).IsEqualTo(12);
            Assert.That(b).IsEqualTo(32.1f);
            Assert.That(c).IsEqualTo("hello");
            Assert.That(d).IsEqualTo(71.231m);
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
        public void InjectsInlineValues([Frozen] object a,
            [Frozen] PropertyHolder<object> value,
            PropertyHolder<object> frozen)
        {
            Assert.That(value.Property).IsEqualTo(a);
            Assert.That(value).IsSameReferenceAs(frozen);
        }
    }
}