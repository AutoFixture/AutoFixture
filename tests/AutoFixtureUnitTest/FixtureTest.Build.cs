using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using AutoFixture;
using AutoFixture.Dsl;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest;

public partial class FixtureTest
{
    [Fact]
    public void FromFactoryWithOneParameterWillRespectPreviousCustomizations()
    {
        // Arrange
        string expectedText = Guid.NewGuid().ToString();
        var sut = new Fixture();
        sut.Customize<PropertyHolder<string>>(ob => ob.With(ph => ph.Property, expectedText));
        // Act
        var result = sut.Build<SingleParameterType<PropertyHolder<string>>>()
            .FromFactory((PropertyHolder<string> ph) => new SingleParameterType<PropertyHolder<string>>(ph))
            .Create();
        // Assert
        Assert.Equal(expectedText, result.Parameter.Property);
    }

    [Fact]
    public void FromFactoryWithTwoParametersWillRespectPreviousCustomizations()
    {
        // Arrange
        string expectedText = Guid.NewGuid().ToString();
        var sut = new Fixture();
        sut.Customize<PropertyHolder<string>>(ob => ob.With(ph => ph.Property, expectedText));
        // Act
        var result = sut.Build<SingleParameterType<PropertyHolder<string>>>()
            .FromFactory((PropertyHolder<string> ph, object dummy) => new SingleParameterType<PropertyHolder<string>>(ph))
            .Create();
        // Assert
        Assert.Equal(expectedText, result.Parameter.Property);
    }

    [Fact]
    public void FromFactoryWithThreeParametersWillRespectPreviousCustomizations()
    {
        // Arrange
        string expectedText = Guid.NewGuid().ToString();
        var sut = new Fixture();
        sut.Customize<PropertyHolder<string>>(ob => ob.With(ph => ph.Property, expectedText));
        // Act
        var result = sut.Build<SingleParameterType<PropertyHolder<string>>>()
            .FromFactory((PropertyHolder<string> ph, object dummy1, object dummy2) => new SingleParameterType<PropertyHolder<string>>(ph))
            .Create();
        // Assert
        Assert.Equal(expectedText, result.Parameter.Property);
    }

    [Fact]
    public void FromFactoryWithFourParametersWillRespectPreviousCustomizations()
    {
        // Arrange
        string expectedText = Guid.NewGuid().ToString();
        var sut = new Fixture();
        sut.Customize<PropertyHolder<string>>(ob => ob.With(ph => ph.Property, expectedText));
        // Act
        var result = sut.Build<SingleParameterType<PropertyHolder<string>>>()
            .FromFactory((PropertyHolder<string> ph, object dummy1, object dummy2, object dummy3) => new SingleParameterType<PropertyHolder<string>>(ph))
            .Create();
        // Assert
        Assert.Equal(expectedText, result.Parameter.Property);
    }

    [Fact]
    public void CustomizeCanDefineConstructor()
    {
        // Arrange
        var sut = new Fixture();
        string expectedText = Guid.NewGuid().ToString();
        sut.Customize<SingleParameterType<string>>(ob => ob.FromFactory(() => new SingleParameterType<string>(expectedText)));
        // Act
        var result = sut.Create<SingleParameterType<string>>();
        // Assert
        Assert.Equal(expectedText, result.Parameter);
    }

    [Fact]
    public void CreateAnonymousWillNotThrowWhenTypeHasIndexedProperty()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        var result = sut.Create<IndexedPropertyHolder<object>>();
        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void BuildWillReturnBuilderThatCreatesTheCorrectNumberOfInstances()
    {
        // Arrange
        int expectedRepeatCount = 242;
        var sut = new Fixture();
        sut.RepeatCount = expectedRepeatCount;
        // Act
        var result = sut.Build<object>().CreateMany();
        // Assert
        Assert.Equal<int>(expectedRepeatCount, result.Count());
    }

    [Fact]
    public void FromSeedWithNullFuncThrows()
    {
        // Arrange
        var sut = new Fixture();
        // Act & assert
        Assert.Throws<ArgumentNullException>(() =>
            sut.Build<object>().FromSeed(null));
    }

    [Fact]
    public void BuildFromSeedWillReturnCorrectResult()
    {
        // Arrange
        var sut = new Fixture();
        var expectedResult = new object();
        // Act
        var result = sut.Build<object>().FromSeed(s => expectedResult).Create();
        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void CustomizeFromSeedWithUnmodifiedSeedValueWillPopulatePropertyOfSameType()
    {
        // Arrange
        var fixture = new Fixture();
        // Act
        fixture.Customize<Version>(c => c.FromSeed(s => s));
        // Assert
        Assert.Null(fixture.Create<PropertyHolder<Version>>().Property);
    }

    [Fact]
    public void CustomizeFromSeedWithFixedSeedValueWillPopulatePropertyOfSameType()
    {
        // Arrange
        var fixture = new Fixture();
        var seed = new ConcreteType();
        // Act
        fixture.Customize<ConcreteType>(c => c.FromSeed(s => seed));
        // Assert
        Assert.Equal(seed, fixture.Create<PropertyHolder<ConcreteType>>().Property);
    }

    [Fact]
    public void BuildAndCreateWillSetInt32Property()
    {
        // Arrange
        int unexpectedNumber = default(int);
        var sut = new Fixture();
        // Act
        PropertyHolder<int> result = sut.Build<PropertyHolder<int>>().Create();
        // Assert
        Assert.NotEqual<int>(unexpectedNumber, result.Property);
    }

    [Fact]
    public void BuildAndCreateWillSetInt32Field()
    {
        // Arrange
        int unexpectedNumber = default(int);
        var sut = new Fixture();
        // Act
        FieldHolder<int> result = sut.Build<FieldHolder<int>>().Create();
        // Assert
        Assert.NotEqual<int>(unexpectedNumber, result.Field);
    }

    [Fact]
    public void BuildAndCreateWillNotAttemptToSetReadOnlyProperty()
    {
        // Arrange
        int expectedNumber = default(int);
        var sut = new Fixture();
        // Act
        ReadOnlyPropertyHolder<int> result = sut.Build<ReadOnlyPropertyHolder<int>>().Create();
        // Assert
        Assert.Equal<int>(expectedNumber, result.Property);
    }

    [Fact]
    public void BuildAndCreateWillNotAttemptToSetReadOnlyField()
    {
        // Arrange
        int expectedNumber = default(int);
        var sut = new Fixture();
        // Act
        ReadOnlyFieldHolder<int> result = sut.Build<ReadOnlyFieldHolder<int>>().Create();
        // Assert
        Assert.Equal<int>(expectedNumber, result.Field);
    }

    [Fact]
    public void BuildWithWillSetPropertyOnCreatedObject()
    {
        // Arrange
        string expectedText = "Anonymous text";
        var sut = new Fixture();
        // Act
        PropertyHolder<string> result = sut.Build<PropertyHolder<string>>()
            .With(ph => ph.Property, expectedText)
            .Create();
        // Assert
        Assert.Equal(expectedText, result.Property);
    }

    [Fact]
    public void BuildWithWillSetFieldOnCreatedObject()
    {
        // Arrange
        string expectedText = "Anonymous text";
        var fixture = new Fixture();
        // Act
        FieldHolder<string> result = fixture
            .Build<FieldHolder<string>>()
            .With(fh => fh.Field, expectedText)
            .Create();
        // Assert
        Assert.Equal(expectedText, result.Field);
    }

    [Fact]
    public void BuildWithFactoryWillSetPropertyOnCreatedObject()
    {
        // Arrange
        var values = new Queue<string>(new[] { "value1", "value2" });
        var fixture = new Fixture();
        // Act
        var builder = fixture
            .Build<PropertyHolder<string>>()
            .With(ph => ph.Property, () => values.Dequeue());
        var result1 = builder.Create();
        var result2 = builder.Create();
        // Assert
        Assert.Equal("value1", result1.Property);
        Assert.Equal("value2", result2.Property);
    }

    [Fact]
    public void BuildWithFactoryWillSetFieldOnCreatedObject()
    {
        // Arrange
        var values = new Queue<string>(new[] { "value1", "value2" });
        var fixture = new Fixture();
        // Act
        var builder = fixture
            .Build<FieldHolder<string>>()
            .With(ph => ph.Field, () => values.Dequeue());
        var result1 = builder.Create();
        var result2 = builder.Create();
        // Assert
        Assert.Equal("value1", result1.Field);
        Assert.Equal("value2", result2.Field);
    }

    [Fact]
    public void BuildWithSingleArgFactoryWillSetPropertyOnCreatedObject()
    {
        // Arrange
        var fixture = new Fixture();
        fixture.Inject<Queue<string>>(new Queue<string>(new[] { "value1", "value2" }));
        // Act
        var builder = fixture
            .Build<PropertyHolder<string>>()
            .With(ph => ph.Property, (Queue<string> values) => values.Dequeue());
        var result1 = builder.Create();
        var result2 = builder.Create();
        // Assert
        Assert.Equal("value1", result1.Property);
        Assert.Equal("value2", result2.Property);
    }

    [Fact]
    public void BuildWithSingleArgFactoryWillSetFieldOnCreatedObject()
    {
        // Arrange
        var fixture = new Fixture();
        fixture.Inject<Queue<string>>(new Queue<string>(new[] { "value1", "value2" }));
        // Act
        var builder = fixture
            .Build<FieldHolder<string>>()
            .With(ph => ph.Field, (Queue<string> values) => values.Dequeue());
        var result1 = builder.Create();
        var result2 = builder.Create();
        // Assert
        Assert.Equal("value1", result1.Field);
        Assert.Equal("value2", result2.Field);
    }

    [Fact]
    public void BuildAnonymousWithWillAssignPropertyEvenInCombinationWithOmitAutoProperties()
    {
        // Arrange
        long unexpectedNumber = default(long);
        var sut = new Fixture();
        // Act
        var result = sut
            .Build<DoublePropertyHolder<long, long>>()
            .With(ph => ph.Property1)
            .OmitAutoProperties()
            .Create();
        // Assert
        Assert.NotEqual<long>(unexpectedNumber, result.Property1);
    }

    [Fact]
    public void BuildWithWillAssignFieldEvenInCombinationWithOmitAutoProperties()
    {
        // Arrange
        int unexpectedNumber = default(int);
        var sut = new Fixture();
        // Act
        var result = sut
            .Build<DoubleFieldHolder<int, decimal>>()
            .With(fh => fh.Field1)
            .OmitAutoProperties()
            .Create();
        // Assert
        Assert.NotEqual<int>(unexpectedNumber, result.Field1);
    }

    [Fact]
    public void BuildWithoutWillIgnorePropertyOnCreatedObject()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        var result = sut.Build<DoublePropertyHolder<string, string>>().Without(ph => ph.Property1).Create();
        // Assert
        Assert.Null(result.Property1);
    }

    [Fact]
    public void BuildWithoutWillIgnorePropertyOnCreatedObjectEvenInCombinationWithWithAutoProperties()
    {
        // Arrange
        var sut = new Fixture() { OmitAutoProperties = true };
        // Act
        var result = sut.Build<DoublePropertyHolder<string, string>>().WithAutoProperties().Without(ph => ph.Property1).Create();
        // Assert
        Assert.Null(result.Property1);
    }

    [Fact]
    public void BuildWithoutWillIgnoreFieldOnCreatedObject()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        var result = sut.Build<DoubleFieldHolder<string, string>>().Without(fh => fh.Field1).Create();
        // Assert
        Assert.Null(result.Field1);
    }

    [Fact]
    public void BuildWithoutWillNotIgnoreOtherPropertyOnCreatedObject()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        var result = sut.Build<DoublePropertyHolder<string, string>>().Without(ph => ph.Property1).Create();
        // Assert
        Assert.NotNull(result.Property2);
    }

    [Fact]
    public void BuildWithoutWillNotIgnoreOtherFieldOnCreatedObject()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        var result = sut.Build<DoubleFieldHolder<string, string>>().Without(fh => fh.Field1).Create();
        // Assert
        Assert.NotNull(result.Field2);
    }

    [Fact]
    public void BuildAndOmitAutoPropertiesWillNotAutoPopulateProperty()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        PropertyHolder<object> result = sut.Build<PropertyHolder<object>>().OmitAutoProperties().Create();
        // Assert
        Assert.Null(result.Property);
    }

    [Fact]
    public void BuildWithAutoPropertiesWillAutoPopulateProperty()
    {
        // Arrange
        var sut = new Fixture { OmitAutoProperties = true };
        // Act
        PropertyHolder<object> result = sut.Build<PropertyHolder<object>>().WithAutoProperties().Create();
        // Assert
        Assert.NotNull(result.Property);
    }

    [Fact]
    public void BuildAndDoWillPerformOperationOnCreatedObject()
    {
        // Arrange
        var sut = new Fixture();
        var expectedObject = new object();
        // Act
        var result = sut.Build<CollectionHolder<object>>().Do(x => x.Collection.Add(expectedObject)).Create().Collection.First();
        // Assert
        Assert.Equal<object>(expectedObject, result);
    }

    [Fact]
    public void AddingTracingBehaviorWillTraceDiagnostics()
    {
        // Arrange
        using (var writer = new StringWriter())
        {
            var sut = new Fixture();
            sut.Behaviors.Add(new TracingBehavior(writer));
            // Act
            sut.Create<int>();
            // Assert
            Assert.False(string.IsNullOrEmpty(writer.ToString()));
        }
    }

    [Fact]
    public void BuildWithOverriddenVirtualPropertyCorrectlySetsProperty()
    {
        // Arrange
        var sut = new Fixture();
        var expected = Guid.NewGuid();
        // Act
        var result = sut.Build<ConcreteType>()
            .With(x => x.Property4, expected)
            .Create();
        // Assert
        Assert.Equal(expected, result.Property4);
    }

    [Fact]
    public void ReturningNullFromFactoryIsPossible()
    {
        var fixture = new Fixture();
        fixture.Customize<string>(x => x.FromFactory(() => null));

        Assert.Null(Record.Exception(() => fixture.Create<string>()));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(10)]
    public void CustomizeBytePropertyReturnsCorrectResult(int expected)
    {
        var fixture = new Fixture();
        fixture.Customize<PropertyHolder<byte>>(c => c
            .With(x => x.Property, expected));

        var actual = fixture.Create<PropertyHolder<byte>>();

        Assert.Equal(expected, actual.Property);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(10)]
    public void CustomizeByteFieldReturnsCorrectResult(int expected)
    {
        var fixture = new Fixture();
        fixture.Customize<FieldHolder<byte>>(c => c
            .With(x => x.Field, expected));

        var actual = fixture.Create<FieldHolder<byte>>();

        Assert.Equal(expected, actual.Field);
    }

    [Fact]
    public void WithImplicitConversionToNullablePropertyReturnsCorrectResult()
    {
        var fixture = new Fixture();
        var expected = fixture.Create<int>();

        var actual = fixture.Build<PropertyHolder<int?>>()
            .With(x => x.Property, expected)
            .Create();

        Assert.Equal(expected, actual.Property);
    }

    [Fact]
    public void WithImplicitConversionToNullableFieldReturnsCorrectResult()
    {
        var fixture = new Fixture();
        var expected = fixture.Create<int>();

        var actual = fixture.Build<FieldHolder<int?>>()
            .With(x => x.Field, expected)
            .Create();

        Assert.Equal(expected, actual.Field);
    }

    /// <summary>
    /// This test reproduces the issue as reported in pull request:
    /// https://github.com/AutoFixture/AutoFixture/pull/604.
    /// </summary>
    [Fact]
    public void WithoutOnFieldInBaseClassThrowsNullPointerException()
    {
        Fixture f = new Fixture();
        f.Build<ConcreteType>().Without(x => x.Field1).Create();

        /*
            Success when no ArgumentNullException is thrown:

            When reported, the call to Without on Field1 residing
            in ConcreteType's base class (AbstractType) will cause
            a null pointer exception to bubble.
        */
    }

    [Fact]
    public void UseActionExpression()
    {
        var fixture = new Fixture();
        var actual = fixture.Create<Expression<Action<string, int>>>();
        Assert.Null(Record.Exception(() => actual.Compile()("foo", 42)));
    }

    [Fact]
    public void CreateFuncExpression()
    {
        var fixture = new Fixture();
        var actual = fixture.Create<Expression<Func<object>>>();
        Assert.NotNull(actual.Compile()());
    }

    [Fact]
    public void BuilderSequenceWillBePreserved()
    {
        // Arrange
        var sut = new Fixture();
        int expectedValue = 3;
        // Act
        var result = sut.Build<PropertyHolder<int>>()
            .With(x => x.Property, 1)
            .Do(x => x.SetProperty(2))
            .With(x => x.Property, expectedValue)
            .Create();
        // Assert
        Assert.Equal<int>(expectedValue, result.Property);
    }

    [Fact]
    public void BuildAndCreateWillInvokeResidueCollector()
    {
        // Arrange
        bool resolveWasInvoked = false;

        var residueCollector = new DelegatingSpecimenBuilder();
        residueCollector.OnCreate = (r, c) =>
        {
            resolveWasInvoked = true;
            return new ConcreteType();
        };

        var sut = new Fixture();
        sut.ResidueCollectors.Add(residueCollector);
        // Act
        sut.Build<PropertyHolder<AbstractType>>().Create();
        // Assert
        Assert.True(resolveWasInvoked, "Resolve");
    }

    [Fact]
    public void BuildAndCreateOnUnregisteredAbstractionWillInvokeResidueCollectorWithCorrectType()
    {
        // Arrange
        var residueCollector = new DelegatingSpecimenBuilder();
        residueCollector.OnCreate = (r, c) =>
        {
            Assert.Equal(typeof(AbstractType), r);
            return new ConcreteType();
        };

        var sut = new Fixture();
        sut.ResidueCollectors.Add(residueCollector);
        // Act
        sut.Build<PropertyHolder<AbstractType>>().Create();
        // Assert (done by callback)
    }

    [Fact]
    public void BuildAndCreateOnUnregisteredAbstractionWillReturnInstanceFromResidueCollector()
    {
        // Arrange
        var expectedValue = new ConcreteType();

        var residueCollector = new DelegatingSpecimenBuilder();
        residueCollector.OnCreate = (r, c) => expectedValue;

        var sut = new Fixture();
        sut.ResidueCollectors.Add(residueCollector);
        // Act
        var result = sut.Build<PropertyHolder<AbstractType>>().Create().Property;
        // Assert
        Assert.Equal<AbstractType>(expectedValue, result);
    }

    [Fact]
    public void BuildAndOmitAutoPropertiesWillNotMutateSut()
    {
        // Arrange
        var fixture = new Fixture();
        var sut = fixture.Build<PropertyHolder<string>>();
        // Act
        sut.OmitAutoProperties();
        // Assert
        var instance = sut.Create();
        Assert.NotNull(instance.Property);
    }

    [Fact]
    public void BuildWithAutoPropertiesWillNotMutateSut()
    {
        // Arrange
        var fixture = new Fixture() { OmitAutoProperties = true };
        var sut = fixture.Build<PropertyHolder<string>>();
        // Act
        sut.WithAutoProperties();
        // Assert
        var instance = sut.Create();
        Assert.Null(instance.Property);
    }

    [Fact]
    public void BuildWithWillNotMutateSut()
    {
        // Arrange
        var fixture = new Fixture();
        var sut = fixture.Build<PropertyHolder<string>>().OmitAutoProperties();
        // Act
        sut.With(s => s.Property);
        // Assert
        var instance = sut.Create();
        Assert.Null(instance.Property);
    }

    [Fact]
    public void BuildWithUnexpectedWillNotMutateSut()
    {
        // Arrange
        var fixture = new Fixture();
        var unexpectedProperty = "Anonymous value";
        var sut = fixture.Build<PropertyHolder<string>>();
        // Act
        sut.With(s => s.Property, unexpectedProperty);
        // Assert
        var instance = sut.Create();
        Assert.NotEqual(unexpectedProperty, instance.Property);
    }

    [Fact]
    public void BuildWithoutWillNotMutateSut()
    {
        // Arrange
        var fixture = new Fixture();
        var sut = fixture.Build<PropertyHolder<string>>();
        // Act
        sut.Without(s => s.Property);
        // Assert
        var instance = sut.Create();
        Assert.NotNull(instance.Property);
    }

    [Fact]
    public void BuildAndCreateWillReturnCreatedObject()
    {
        // Arrange
        object expectedObject = new object();
        var sut = new Fixture();
        // Act
        object result = sut.Build<object>().FromSeed(seed => expectedObject).Create();
        // Assert
        Assert.Equal<object>(expectedObject, result);
    }

    [Fact]
    public void BuildAndCreateManyWillCreateManyAnonymousItems()
    {
        // Arrange
        var sut = new Fixture();
        var expectedItemCount = sut.RepeatCount;
        // Act
        IEnumerable<PropertyHolder<int>> result = sut.Build<PropertyHolder<int>>().CreateMany();
        // Assert
        var uniqueItemCount = (from ph in result
            select ph.Property).Distinct().Count();
        Assert.Equal<int>(expectedItemCount, uniqueItemCount);
    }

    [Fact]
    public void BuildAndCreateManyWillCreateCorrectNumberOfItems()
    {
        // Arrange
        int expectedCount = 401;
        var sut = new Fixture();
        // Act
        IEnumerable<PropertyHolder<int>> result = sut.Build<PropertyHolder<int>>().CreateMany(expectedCount);
        // Assert
        var uniqueItemCount = (from ph in result
            select ph.Property).Distinct().Count();
        Assert.Equal<int>(expectedCount, uniqueItemCount);
    }

    [Fact]
    public void BuildAndCreateWillCreateObject()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        object result = sut.Build<object>().Create();
        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void BuildAndCreateAfterDefiningConstructorWithZeroParametersWillReturnDefinedObject()
    {
        // Arrange
        var sut = new Fixture();
        object expectedObject = new object();
        // Act
        var result = sut.Build<object>()
            .FromFactory(() => expectedObject)
            .Create();
        // Assert
        Assert.Equal<object>(expectedObject, result);
    }

    [Fact]
    public void BuildAndCreateAfterDefiningConstructorWithOneParameterWillReturnDefinedObject()
    {
        // Arrange
        var sut = new Fixture();
        SingleParameterType<object> expectedObject = new SingleParameterType<object>(new object());
        // Act
        var result = sut.Build<SingleParameterType<object>>()
            .FromFactory<object>(obj => expectedObject)
            .Create();
        // Assert
        Assert.Equal<SingleParameterType<object>>(expectedObject, result);
    }

    [Fact]
    public void BuildAndCreateAfterDefiningConstructorWithTwoParametersWillReturnDefinedObject()
    {
        // Arrange
        var sut = new Fixture();
        DoubleParameterType<object, object> expectedObject = new DoubleParameterType<object, object>(new object(), new object());
        // Act
        var result = sut.Build<DoubleParameterType<object, object>>()
            .FromFactory<object, object>((o1, o2) => expectedObject)
            .Create();
        // Assert
        Assert.Equal<DoubleParameterType<object, object>>(expectedObject, result);
    }

    [Fact]
    public void BuildAndCreateAfterDefiningConstructorWithThreeParametersWillReturnDefinedObject()
    {
        // Arrange
        var sut = new Fixture();
        TripleParameterType<object, object, object> expectedObject = new TripleParameterType<object, object, object>(new object(), new object(), new object());
        // Act
        var result = sut.Build<TripleParameterType<object, object, object>>()
            .FromFactory<object, object, object>((o1, o2, o3) => expectedObject)
            .Create();
        // Assert
        Assert.Equal<TripleParameterType<object, object, object>>(expectedObject, result);
    }

    [Fact]
    public void BuildAndCreateAfterDefiningConstructorWithFourParametersWillReturnDefinedObject()
    {
        // Arrange
        var sut = new Fixture();
        QuadrupleParameterType<object, object, object, object> expectedObject = new QuadrupleParameterType<object, object, object, object>(new object(), new object(), new object(), new object());
        // Act
        var result = sut.Build<QuadrupleParameterType<object, object, object, object>>()
            .FromFactory<object, object, object, object>((o1, o2, o3, o4) => expectedObject)
            .Create();
        // Assert
        Assert.Equal<QuadrupleParameterType<object, object, object, object>>(expectedObject, result);
    }

    [Fact]
    public void BuildFromFactoryStillAppliesAutoProperties()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        var result = sut.Build<PropertyHolder<string>>()
            .FromFactory(() => new PropertyHolder<string>())
            .Create();
        // Assert
        Assert.NotNull(result.Property);
    }

    [Fact]
    public void BuildOverwritesPreviousFactoryBasedCustomization()
    {
        // Arrange
        var sut = new Fixture();
        sut.Customize<PropertyHolder<object>>(c => c.FromFactory(() => new PropertyHolder<object>()));
        // Act
        var result = sut.Build<PropertyHolder<object>>().OmitAutoProperties().Create();
        // Assert
        Assert.Null(result.Property);
    }

    [Fact]
    public void NewestCustomizationWins()
    {
        // Arrange
        var sut = new Fixture();
        sut.Customize<string>(c => c.FromFactory(() => "ploeh"));

        var expectedResult = "fnaah";
        // Act
        sut.Customize<string>(c => c.FromFactory(() => expectedResult));
        var result = sut.Create<string>();
        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void BuildAndComposeWillCarryBehaviorsForward()
    {
        // Arrange
        var sut = new Fixture();
        sut.Behaviors.Clear();

        var expectedBuilder = new DelegatingSpecimenBuilder();
        sut.Behaviors.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode(1, b) });
        // Act
        var result = sut.Build<object>();
        // Assert
        var comparer = new TaggedNodeComparer(new TrueComparer<ISpecimenBuilder>());
        var composite = Assert.IsAssignableFrom<CompositeNodeComposer<object>>(result);
        Assert.Equal(new TaggedNode(1), composite.Node.First(), comparer);
    }

    [Fact]
    public void BuildAbstractClassThrows()
    {
        // Arrange
        var sut = new Fixture();
        // Act & assert
        Assert.Throws<ObjectCreationException>(() =>
            sut.Build<AbstractType>().Create());
    }

    [Fact]
    public void BuildAbstractTypeUsingStronglyTypedFactoryIsPossible()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        var result = sut.Build<AbstractType>().FromFactory(() => new ConcreteType()).Create();
        // Assert
        Assert.IsAssignableFrom<ConcreteType>(result);
    }

    [Fact]
    public void BuildAbstractTypeUsingBuilderIsPossible()
    {
        // Arrange
        var sut = new Fixture();
        var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => new ConcreteType() };
        // Act
        var result = sut.Build<AbstractType>().FromFactory(builder).Create();
        // Assert
        Assert.IsAssignableFrom<ConcreteType>(result);
    }

    [Fact]
    public void BuildAbstractTypeCorrectlyAppliesProperty()
    {
        // Arrange
        var expected = new object();
        var sut = new Fixture();
        // Act
        var result = sut.Build<AbstractType>()
            .FromFactory(() => new ConcreteType())
            .With(x => x.Property1, expected)
            .Create();
        // Assert
        Assert.Equal(expected, result.Property1);
    }
}
