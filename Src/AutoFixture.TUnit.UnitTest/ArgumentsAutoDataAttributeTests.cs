using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoFixture.TUnit.UnitTest.TestTypes;
using TestTypeFoundation;

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
            Assert.False(wasInvoked);
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
                .GetMethod(methodName, new[] { typeof(ConcreteType) });
            var customizationLog = new List<ICustomization>();
            var fixture = new DelegatingFixture
            {
                OnCustomize = c => customizationLog.Add(c)
            };
            var sut = new DerivedArgumentsAutoDataAttribute(() => fixture);

            // Act
            _ = await sut.GenerateDataSources(new DataGeneratorMetadata
            {
                
            });

            // Assert
            var composite = Assert.That(customizationLog[0]).IsAssignableFrom<CompositeCustomization>();
            Assert.That(composite.Customizations.First()).IsNotTypeOf<FreezeOnMatchCustomization>;
            Assert.That(composite.Customizations.Last()).IsTypeOf<FreezeOnMatchCustomization>();
        }

        [Test]
        [ClassData(typeof(InlinePrimitiveValuesTestData))]
        [ClassData(typeof(InlineFrozenValuesTestData))]
        public async Task ReturnsSingleTestDataWithExpectedValues(DataAttribute attribute, MethodInfo testMethod,
            object[] expected)
        {
            // Act
            var actual = (await attribute.GetData(testMethod!, new DisposalTracker()))
                    .Select(x => x.GetData()).ToArray();

            // Assert
            Assert.Single(actual);
            Assert.Equal(expected, actual[0]);
        }

        [Test]
        [ArgumentsAutoData]
        public void GeneratesRandomData(int a, float b, string c, decimal d)
        {
            Assert.NotEqual(0, a);
            Assert.NotEqual(0, b);
            Assert.NotNull(c);
            Assert.NotEqual(0, d);
        }

        [Test]
        [ArgumentsAutoData(12, 32.1f, "hello", 71.231d)]
        public void InlinesAllData(int a, float b, string c, decimal d)
        {
            Assert.Equal(12, a);
            Assert.Equal(32.1f, b);
            Assert.Equal("hello", c);
            Assert.Equal(71.231m, d);
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
        [ArgumentsAutoData(new object[] { null })]
        public void InjectsInlineValues([Frozen] object a,
            [Frozen] PropertyHolder<object> value,
            PropertyHolder<object> frozen)
        {
            Assert.Equal(a, value.Property);
            Assert.Same(frozen, value);
        }
    }
}