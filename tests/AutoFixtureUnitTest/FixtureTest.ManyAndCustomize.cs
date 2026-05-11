using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using TestTypeFoundation;
using Xunit;
using MethodInvoker = AutoFixture.Kernel.MethodInvoker;

namespace AutoFixtureUnitTest;

public partial class FixtureTest
{
    [Fact]
    public void DefaultRepeatCountIsThree()
    {
        // Arrange
        int expectedRepeatCount = 3;
        Fixture sut = new Fixture();
        // Act
        int result = sut.RepeatCount;
        // Assert
        Assert.Equal<int>(expectedRepeatCount, result);
    }

    [Fact]
    public void RepeatWillPerformActionTheDefaultNumberOfTimes()
    {
        // Arrange
        IFixture sut = new Fixture();
        int expectedCount = sut.RepeatCount;
        // Act
        int result = 0;
        sut.Repeat(() => result++).ToList();
        // Assert
        Assert.Equal<int>(expectedCount, result);
    }

    [Fact]
    public void RepeatWillReturnTheDefaultNumberOfItems()
    {
        // Arrange
        IFixture sut = new Fixture();
        int expectedCount = sut.RepeatCount;
        // Act
        IEnumerable<object> result = sut.Repeat(() => new object());
        // Assert
        Assert.Equal<int>(expectedCount, result.Count());
    }

    [Fact]
    public void RepeatWillPerformActionTheSpecifiedNumberOfTimes()
    {
        // Arrange
        int expectedCount = 2;
        IFixture sut = new Fixture();
        sut.RepeatCount = expectedCount;
        // Act
        int result = 0;
        sut.Repeat(() => result++).ToList();
        // Assert
        Assert.Equal<int>(expectedCount, result);
    }

    [Fact]
    public void RepeatWillReturnTheSpecifiedNumberOfItems()
    {
        // Arrange
        int expectedCount = 13;
        IFixture sut = new Fixture();
        sut.RepeatCount = expectedCount;
        // Act
        IEnumerable<object> result = sut.Repeat(() => new object());
        // Assert
        Assert.Equal<int>(expectedCount, result.Count());
    }

    [Fact]
    public void ReplacingStringMappingWillUseNewStringCreationAlgorithm()
    {
        // Arrange
        string expectedText = "Anonymous string";
        Fixture sut = new Fixture();
        // Act
        sut.Customize<string>(c => c.FromSeed(s => expectedText));
        // Assert
        string result = sut.Create<string>();
        Assert.Equal(expectedText, result);
    }

    [Fact]
    public void AddManyWillAddItemsToListUsingCreator()
    {
        // Arrange
        Fixture sut = new Fixture();
        IEnumerable<int> expectedList = Enumerable.Range(1, sut.RepeatCount);
        List<int> list = new List<int>();
        // Act
        int i = 0;
        sut.AddManyTo(list, () => ++i);
        // Assert
        Assert.True(expectedList.SequenceEqual(list));
    }

    [Fact]
    public void AddManyWillAddItemsToListUsingAnonymousCreator()
    {
        // Arrange
        Fixture sut = new Fixture();
        int expectedItemCount = sut.RepeatCount;
        List<string> list = new List<string>();
        // Act
        sut.AddManyTo(list);
        // Assert
        int result = (from s in list
            where !string.IsNullOrEmpty(s)
            select s).Count();
        Assert.Equal<int>(expectedItemCount, result);
    }

    [Fact]
    public void AddManyWillAddItemsToCollection()
    {
        // Arrange
        Fixture sut = new Fixture();
        int expectedCount = sut.RepeatCount;
        ICollection<int> collection = new LinkedList<int>();
        // Act
        sut.AddManyTo(collection);
        // Assert
        Assert.Equal<int>(expectedCount, collection.Count);
    }

    [Fact]
    public void AddManyWithRepeatCountWillAddItemsToCollection()
    {
        // Arrange
        var sut = new Fixture();
        int expectedCount = 24;
        ICollection<int> collection = new LinkedList<int>();
        // Act
        sut.AddManyTo(collection, expectedCount);
        // Assert
        Assert.Equal<int>(expectedCount, collection.Count);
    }

    [Fact]
    public void AddManyWithCreatorWillAddItemsToCollection()
    {
        // Arrange
        Fixture sut = new Fixture();
        int expectedCount = sut.RepeatCount;
        ICollection<object> collection = new LinkedList<object>();
        // Act
        sut.AddManyTo(collection, () => new object());
        // Assert
        Assert.Equal<int>(expectedCount, collection.Count);
    }

    [Fact]
    public void CreateManyWillCreateManyAnonymousItems()
    {
        // Arrange
        Fixture sut = new Fixture();
        int expectedItemCount = sut.RepeatCount;
        // Act
        IEnumerable<string> result = sut.CreateMany<string>();
        // Assert
        int nonDefaultCount = (from s in result
            where !string.IsNullOrEmpty(s)
            select s).Count();
        Assert.Equal<int>(expectedItemCount, nonDefaultCount);
    }

    [Fact]
    public void CreateManyWillCreateCorrectNumberOfAnonymousItems()
    {
        // Arrange
        var sut = new Fixture();
        int expectedItemCount = 248;
        // Act
        IEnumerable<string> result = sut.CreateMany<string>(expectedItemCount);
        // Assert
        int nonDefaultCount = (from s in result
            where !string.IsNullOrEmpty(s)
            select s).Count();
        Assert.Equal<int>(expectedItemCount, nonDefaultCount);
    }

    [Fact]
    public void CustomizeNullTransformationThrows()
    {
        // Arrange
        var sut = new Fixture();
        // Act & assert
        Assert.Throws<ArgumentNullException>(() =>
            sut.Customize<object>(null));
    }

    [Fact]
    public void RegisterTypeWithPropertyOverrideWillSetPropertyValueCorrectly()
    {
        // Arrange
        string expectedValue = "Anonymous text";
        Fixture sut = new Fixture();
        // Act
        sut.Customize<PropertyHolder<string>>(f => f.With(ph => ph.Property, expectedValue));
        PropertyHolder<string> result = sut.Create<PropertyHolder<string>>();
        // Assert
        Assert.Equal(expectedValue, result.Property);
    }

    [Fact]
    public void RegisterTypeWithElementsBuilderSetsPropertyValueCorrectly()
    {
        // Arrange
        var values = new[] { "one", "two", "three" };
        var builder = new ElementsBuilder<string>(values);
        var sut = new Fixture();
        // Act
        sut.Customize<PropertyHolder<string>>(f => f.With(ph => ph.Property, builder));
        var result = sut.CreateMany<PropertyHolder<string>>();

        // Assert
        Assert.All(result, result =>
        {
            Assert.Contains(result.Property, values);
        });
    }

    [Fact]
    public void RegisterTypeWithFixedPropertyBuilderSetsPropertyValueCorrectly()
    {
        // Arrange
        var builder = new FixedBuilder("hello-world");
        var sut = new Fixture();
        // Act
        sut.Customize<PropertyHolder<string>>(f => f.With(ph => ph.Property, builder));
        var result = sut.Create<PropertyHolder<string>>();

        // Assert
        Assert.Equal("hello-world", result.Property);
    }

    [Fact]
    public void RegisterTypeWithBuilderCanResolveValuesFromUnderlyingContext()
    {
        // Arrange
        var builder = new DelegatingSpecimenBuilder
        {
            OnCreate = (r, c) => c.Resolve(r)
        };
        var sut = new Fixture();
        var frozenValue = sut.Freeze<string>();
        // Act
        sut.Customize<PropertyHolder<string>>(f => f.With(ph => ph.Property, builder));
        var result = sut.Create<PropertyHolder<string>>();

        // Assert
        Assert.Equal(frozenValue, result.Property);
    }

    [Fact]
    public void RegisterTypeWithDefaultPrimitiveBuilderSetsPropertyValueCorrectly()
    {
        // Arrange
        var builder = new CompositeSpecimenBuilder(new DefaultPrimitiveBuilders());
        var sut = new Fixture();
        // Act
        sut.Customize<PropertyHolder<string>>(f => f.With(ph => ph.Property, builder));
        var result = sut.Create<PropertyHolder<string>>();

        // Assert
        Assert.True(Guid.TryParse(result.Property, out var _));
    }

    [Fact]
    public void RegisterTypeWithBuilderReturningIncorrectTypeThrowsCastException()
    {
        // Arrange
        var builder = new FixedBuilder(42);
        var sut = new Fixture();

        // Act
        sut.Customize<PropertyHolder<string>>(f => f.With(ph => ph.Property, builder));

        // Assert
        var exception = Record.Exception(() => sut.Create<PropertyHolder<string>>());
        Assert.IsAssignableFrom<ObjectCreationException>(exception);
        Assert.IsType<InvalidCastException>(exception.InnerException);
    }

    [Fact]
    public void RegisteringTypeWithPropertyBuilderRespectsOmittingAutoProperties()
    {
        // Arrange
        var builder = new FixedBuilder("some-string");
        var sut = new Fixture();

        // Act
        sut.Customize<DoublePropertyHolder<string, string>>(f => f.With(ph => ph.Property1, builder).OmitAutoProperties());
        var actual = sut.Create<DoublePropertyHolder<string, string>>();

        // Assert
        Assert.Equal("some-string", actual.Property1);
        Assert.Null(actual.Property2);
    }

    [Fact]
    public void CreateNestedTypeWillPopulateNestedProperty()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        var result = sut.Create<PropertyHolder<PropertyHolder<string>>>();
        // Assert
        Assert.False(string.IsNullOrEmpty(result.Property.Property), "Nested property string should not be null or empty.");
    }

    [Fact]
    public void RegisterNullWillAssignCorrectPickedPropertyValue()
    {
        // Arrange
        var sut = new Fixture();
        sut.Register(() => (string)null);
        // Act
        var result = sut.Build<PropertyHolder<string>>().With(p => p.Property).Create();
        // Assert
        Assert.Null(result.Property);
    }

    [Fact]
    public void CustomizeNullCustomizationThrows()
    {
        // Arrange
        var sut = new Fixture();
        // Act & assert
        Assert.Throws<ArgumentNullException>(() =>
            sut.Customize((ICustomization)null));
    }

    [Fact]
    public void CustomizeCorrectlyAppliesCustomization()
    {
        // Arrange
        var sut = new Fixture();

        var verified = false;
        var customization = new DelegatingCustomization { OnCustomize = f => verified = f == sut };
        // Act
        sut.Customize(customization);
        // Assert
        Assert.True(verified, "Mock verified");
    }

    [Fact]
    public void CustomizeReturnsCorrectResult()
    {
        // Arrange
        var sut = new Fixture();
        var dummyCustomization = new DelegatingCustomization();
        // Act
        var result = sut.Customize(dummyCustomization);
        // Assert
        Assert.Equal(sut, result);
    }

    [Fact]
    public void ItIsPossibleToMapManyRequestToCustomEnumerableInstance()
    {
        // Arrange
        var sut = new Fixture();
        var expected = new[] { "a", "b", "c", "d" };
        sut.Customizations.Add(
            new FilteringSpecimenBuilder(
                new FixedBuilder(
                    expected),
                new EqualRequestSpecification(
                    new MultipleRequest(
                        new SeededRequest(
                            typeof(string),
                            default(string))))));

        // Act
        var actual = sut.CreateMany<string>();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CustomizationOfOverriddenPropOfChildADoesNotAffectChildB()
    {
        // Arrange
        var fixture = new Fixture();
        fixture.Customize<AcwaacpChildA>(c => c.Without(x => x.Value));
        // Act
        var actual = fixture.Create<AcwaacpChildB>();
        // Assert
        Assert.NotEqual(default(int), actual.Value);
    }

    /// <summary>
    /// Checks the scenario: https://github.com/AutoFixture/AutoFixture/issues/531.
    /// </summary>
    [Fact]
    public void CustomizationOfBasePropOfChildADoesNotAffectChildB()
    {
        // arrange
        var sut = new Fixture();
        sut.Customize<AcwaacpChildA>(c => c.With(x => x.Text, "foo"));

        // act
        var actual = sut.Create<AcwaacpChildB>();

        // assert
        Assert.NotNull(actual.Text);
        Assert.NotEqual("foo", actual.Text);
    }

    /// <summary>
    /// Checks the scenario reported in https://github.com/AutoFixture/AutoFixture/issues/772.
    /// </summary>
    [Fact]
    public void CustomizatonOfSamePropertyIsIgnoredDuringTheBuild()
    {
        // arrange
        var sut = new Fixture();
        sut.Customize<DoublePropertyHolder<string, int>>(c => c.With(x => x.Property1, "foo"));

        // act
        var result = sut
            .Build<DoublePropertyHolder<string, int>>()
            .With(x => x.Property2, 42)
            .Create();

        // assert
        Assert.NotEqual("foo", result.Property1);
        Assert.Equal(42, result.Property2);
    }

    /// <summary>
    /// Scenario from https://github.com/AutoFixture/AutoFixture/issues/321.
    /// </summary>
    [Fact]
    public void CustomizationOfIntPropertyDoesntThrowInBuild()
    {
        // arrange
        var sut = new Fixture();
        sut.Customize<PropertyHolder<long>>(c => c.Without(x => x.Property));

        // act
        var result = sut.Build<PropertyHolder<long>>().With(x => x.Property).Create();

        // assert
        Assert.NotEqual(0L, result.Property);
    }

    [Fact]
    public void EnableDisableAutoPropertiesDoesntBreakCustomization()
    {
        // arrange
        var sut = new Fixture();
        sut.Customize<PropertyHolder<string>>(c =>
            c
                .Without(x => x.Property)
                .OmitAutoProperties()
                .WithAutoProperties());

        // act
        var result = sut.Create<PropertyHolder<string>>();

        // assert
        Assert.Null(result.Property);
    }

    [Fact]
    public void UseGreedyConstructorQueryWithAutoProperties()
    {
        // Arrange
        var fixture = new Fixture();
        fixture.Customizations.Add(new Postprocessor(
            new MethodInvoker(new GreedyConstructorQuery()),
            new AutoPropertiesCommand(),
            new AnyTypeSpecification()));
        // Act
        var result = fixture.Create<ConcreteType>();
        // Assert
        Assert.NotNull(result.Property5);
    }

    private abstract class AbstractClassWithAbstractAndConcreteProperties
    {
        public string Text { get; set; }

        public abstract int Value { get; set; }
    }

    private class AcwaacpChildA : AbstractClassWithAbstractAndConcreteProperties
    {
        public override int Value { get; set; }
    }

    private class AcwaacpChildB : AbstractClassWithAbstractAndConcreteProperties
    {
        public override int Value { get; set; }
    }
}
