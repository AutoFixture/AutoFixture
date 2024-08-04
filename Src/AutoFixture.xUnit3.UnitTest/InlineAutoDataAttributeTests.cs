using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture.Xunit2.UnitTest.TestTypes;
using TestTypeFoundation;
using Xunit;
using Xunit.Sdk;

namespace AutoFixture.Xunit2.UnitTest
{
    public class InlineAutoDataAttributeTests
    {
        [Fact]
        public void SutIsDataAttribute()
        {
            // Arrange & Act
            var sut = new InlineAutoDataAttribute();

            // Assert
            Assert.IsAssignableFrom<DataAttribute>(sut);
        }

        [Fact]
        public void ValuesWillBeEmptyWhenSutIsCreatedWithDefaultConstructor()
        {
            // Arrange
            var sut = new InlineAutoDataAttribute();
            var expected = Enumerable.Empty<object>();

            // Act
            var result = sut.Values;

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ValuesWillNotBeEmptyWhenSutIsCreatedWithConstructorArguments()
        {
            // Arrange
            var expectedValues = new[] { new object(), new object(), new object() };
            var sut = new InlineAutoDataAttribute(expectedValues);

            // Act
            var result = sut.Values;

            // Assert
            Assert.True(result.SequenceEqual(expectedValues));
        }

        [Fact]
        public void ValuesAreCorrectWhenConstructedWithExplicitAutoDataAttribute()
        {
            // Arrange
            var expectedValues = new[] { new object(), new object(), new object() };
            var sut = new DerivedInlineAutoDataAttribute(() => new DelegatingFixture(), expectedValues);

            // Act
            var result = sut.Values;

            // Assert
            Assert.Equal(expectedValues, result);
        }

        [Fact]
        public void DoesntActivateFixtureImmediately()
        {
            // Arrange
            var wasInvoked = false;
            Func<IFixture> autoData = () =>
            {
                wasInvoked = true;
                return new DelegatingFixture();
            };

            // Act
            _ = new DerivedInlineAutoDataAttribute(autoData);

            // Assert
            Assert.False(wasInvoked);
        }

        [Fact]
        public void PreDiscoveryShouldBeDisabled()
        {
            // Arrange
            var expectedDiscovererType = typeof(NoPreDiscoveryDataDiscoverer).GetTypeInfo();
            var discovererAttr = typeof(InlineAutoDataAttribute).GetTypeInfo()
                .CustomAttributes
                .Single(x => x.AttributeType == typeof(DataDiscovererAttribute));

            var expectedType = expectedDiscovererType.FullName;
            var expectedAssembly = expectedDiscovererType.Assembly.GetName().Name;

            // Act
            var actualType = (string)discovererAttr.ConstructorArguments[0].Value;
            var actualAssembly = (string)discovererAttr.ConstructorArguments[1].Value;

            // Assert
            Assert.Equal(expectedType, actualType);
            Assert.Equal(expectedAssembly, actualAssembly);
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
            var sut = new DerivedInlineAutoDataAttribute(() => fixture);

            // Act
            sut.GetData(method).ToArray();

            // Assert
            var composite = Assert.IsAssignableFrom<CompositeCustomization>(customizationLog[0]);
            Assert.IsNotType<FreezeOnMatchCustomization>(composite.Customizations.First());
            Assert.IsType<FreezeOnMatchCustomization>(composite.Customizations.Last());
        }

        [Theory]
        [ClassData(typeof(InlinePrimitiveValuesTestData))]
        [ClassData(typeof(InlineFrozenValuesTestData))]
        public void ReturnsSingleTestDataWithExpectedValues(
            DataAttribute attribute, MethodInfo testMethod, object[] expected)
        {
            // Act
            var actual = attribute.GetData(testMethod).ToArray();

            // Assert
            Assert.Single(actual);
            Assert.Equal(expected, actual[0]);
        }

        [Theory]
        [InlineAutoData]
        public void GeneratesRandomData(int a, float b, string c, decimal d)
        {
            Assert.NotEqual(default, a);
            Assert.NotEqual(default, b);
            Assert.NotEqual(default, c);
            Assert.NotEqual(default, d);
        }

        [Theory]
        [InlineAutoData(12, 32.1f, "hello", 71.231d)]
        public void InlinesAllData(int a, float b, string c, decimal d)
        {
            Assert.Equal(12, a);
            Assert.Equal(32.1f, b);
            Assert.Equal("hello", c);
            Assert.Equal(71.231m, d);
        }

        [Theory]
        [InlineAutoData(0)]
        [InlineAutoData(5)]
        [InlineAutoData(-12)]
        [InlineAutoData(21.3f)]
        [InlineAutoData(18.7d)]
        [InlineAutoData(EnumType.First)]
        [InlineAutoData("Hello World")]
        [InlineAutoData("\t\r\n")]
        [InlineAutoData(" ")]
        [InlineAutoData("")]
        [InlineAutoData(null)]
        public void InjectsInlineValues(
            [Frozen] object a,
            [Frozen] PropertyHolder<object> value,
            PropertyHolder<object> frozen)
        {
            Assert.Equal(a, value.Property);
            Assert.Same(frozen, value);
        }
    }
}