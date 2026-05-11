using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
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
    public void InitializedWithDefaultConstructorSutHasCorrectEngineParts()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        var result = sut.Engine;
        // Assert
        var expectedParts = from b in new DefaultEngineParts()
            select b.GetType();
        var composite = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(result);
        Assert.True(expectedParts.SequenceEqual(from b in composite
            select b.GetType()));
    }

    [Fact]
    public void InitializeWithNullRelaysThrows()
    {
        // Arrange
        // Act & assert
        var ex = Assert.Throws<ArgumentNullException>(() =>
            new Fixture(null));
        Assert.Equal("engineParts", ex.ParamName);
    }

    [Fact]
    public void InitializedWithRelaysSutHasCorrectEngineParts()
    {
        // Arrange
        var relays = new DefaultRelays();
        var sut = new Fixture(relays);
        // Act
        var result = sut.Engine;
        // Assert
        var expectedParts = from b in relays
            select b.GetType();
        var composite = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(result);
        Assert.True(expectedParts.SequenceEqual(from b in composite
            select b.GetType()));
    }

    [Fact]
    public void InitializeWithNullEngineThrows()
    {
        // Arrange
        var dummyMany = new MultipleRelay();
        // Act & assert
        Assert.Throws<ArgumentNullException>(() =>
            new Fixture(null, dummyMany));
    }

    [Fact]
    public void InitializeWithNullManyThrows()
    {
        // Arrange
        var dummyBuilder = new DelegatingSpecimenBuilder();
        // Act & assert
        Assert.Throws<ArgumentNullException>(() =>
            new Fixture(dummyBuilder, null));
    }

    [Fact]
    public void InitializedWithEngineSutHasCorrectEngine()
    {
        // Arrange
        var expectedEngine = new DelegatingSpecimenBuilder();
        var dummyMany = new MultipleRelay();
        var sut = new Fixture(expectedEngine, dummyMany);
        // Act
        var result = sut.Engine;
        // Assert
        Assert.Equal(expectedEngine, result);
    }

    [Fact]
    public void InitializedWithManySutHasCorrectRepeatCount()
    {
        // Arrange
        var expectedRepeatCount = 187;
        var dummyBuilder = new DelegatingSpecimenBuilder();
        var many = new MultipleRelay { Count = expectedRepeatCount };
        var sut = new Fixture(dummyBuilder, many);
        // Act
        var result = sut.RepeatCount;
        // Assert
        Assert.Equal(expectedRepeatCount, result);
    }

    [Fact]
    public void SettingRepeatCountWillCorrectlyUpdateMany()
    {
        // Arrange
        var dummyBuilder = new DelegatingSpecimenBuilder();
        var many = new MultipleRelay();
        var sut = new Fixture(dummyBuilder, many);
        // Act
        sut.RepeatCount = 26;
        // Assert
        Assert.Equal(sut.RepeatCount, many.Count);
    }

    [Fact]
    public void CustomizationsIsInstance()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        IList<ISpecimenBuilder> result = sut.Customizations;
        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void CustomizationsIsStable()
    {
        // Arrange
        var sut = new Fixture();
        var builder = new DelegatingSpecimenBuilder();
        // Act
        sut.Customizations.Add(builder);
        // Assert
        Assert.Contains(builder, sut.Customizations);
    }

    [Fact]
    public void ResidueCollectorsIsInstance()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        IList<ISpecimenBuilder> result = sut.ResidueCollectors;
        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void ResidueCollectorsIsStable()
    {
        // Arrange
        var sut = new Fixture();
        var builder = new DelegatingSpecimenBuilder();
        // Act
        sut.ResidueCollectors.Add(builder);
        // Assert
        Assert.Contains(builder, sut.ResidueCollectors);
    }

    [Fact]
    public void BehaviorsIsInstance()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        IList<ISpecimenBuilderTransformation> result = sut.Behaviors;
        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void BehaviorsIsStable()
    {
        // Arrange
        var sut = new Fixture();
        var behavior = new DelegatingSpecimenBuilderTransformation();
        // Act
        sut.Behaviors.Add(behavior);
        // Assert
        Assert.Contains(behavior, sut.Behaviors);
    }

    [Fact]
    public void BehaviorsContainsCorrectRecursionBehavior()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        var result = sut.Behaviors;
        // Assert
        Assert.True(result.OfType<ThrowingRecursionBehavior>().Any());
    }

    [Fact]
    public void SutIsCustomizableComposer()
    {
        // Arrange
        // Act
        var sut = new Fixture();
        // Assert
        Assert.IsAssignableFrom<IFixture>(sut);
    }

    [Fact]
    public void CreateAnonymousWillCreateSimpleObject()
    {
        // Arrange
        Fixture sut = new Fixture();
        // Act
        object result = sut.Create<object>();
        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void CreateUnregisteredAbstractTypeWillThrow()
    {
        // Arrange
        Fixture sut = new Fixture();
        // Act & assert
        Assert.ThrowsAny<ObjectCreationException>(() =>
            sut.Create<AbstractType>());
    }

    [Fact]
    public void CreateAnonymousWillCreateSingleParameterType()
    {
        // Arrange
        Fixture sut = new Fixture();
        // Act
        SingleParameterType<object> result = sut.Create<SingleParameterType<object>>();
        // Assert
        Assert.NotNull(result.Parameter);
    }

    [Fact]
    public void CreateAnonymousWillUseRegisteredMapping()
    {
        // Arrange
        Fixture sut = new Fixture();
        sut.Register<AbstractType>(() => new ConcreteType());
        // Act
        SingleParameterType<AbstractType> result = sut.Create<SingleParameterType<AbstractType>>();
        // Assert
        Assert.IsAssignableFrom<ConcreteType>(result.Parameter);
    }

    [Fact]
    public void CreateOnMultipleThreadsConcurrentlyGeneratesPopulatedSpecimens()
    {
        // Arrange
        const int specimenCountPerThread = 25;
        const int threadCount = 8;
        var sut = new Fixture();

        // Act
        IEnumerable<object> GetPropertyAndFieldValues(object obj, BindingFlags flags)
        {
            var type = obj.GetType();
            foreach (var fieldInfo in type.GetFields(flags))
            {
                yield return fieldInfo.GetValue(obj);
            }

            foreach (var propertyInfo in type.GetProperties(flags))
            {
                yield return propertyInfo.GetValue(obj);
            }
        }

        var specimensByThread = Enumerable.Range(0, threadCount)
            .AsParallel()
            .WithDegreeOfParallelism(threadCount)
            .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
            .Select(threadNumber => Enumerable
                .Range(0, specimenCountPerThread)
                .Select(_ => sut.Create<SpecimenWithEverything>())
                .Select(s => new
                {
                    Specimen = s,
                    threadNumber,
                    ValuesNotPopulated = GetPropertyAndFieldValues(s, BindingFlags.Public | BindingFlags.Instance)
                        .Where(v => v == null || 0.Equals(v))
                        .ToArray()
                })
                .ToArray())
            .ToArray();

        // Assert
        Assert.Equal(specimenCountPerThread * threadCount, specimensByThread.Sum(t => t.Length));

        var allValuesNotPopulated = specimensByThread
            .SelectMany(t => t.SelectMany(s => s.ValuesNotPopulated));

        Assert.Empty(allValuesNotPopulated);
    }

    [Fact]
    public void CreateAnonymousWillUseRegisteredMappingWithSingleParameter()
    {
        // Arrange
        Fixture sut = new Fixture();
        sut.Register<object, AbstractType>(obj => new ConcreteType(obj));
        // Act
        AbstractType result = sut.Create<AbstractType>();
        // Assert
        Assert.NotNull(result.Property1);
    }

    [Fact]
    public void CreateAnonymousWillUseRegisteredMappingWithDoubleParameters()
    {
        // Arrange
        Fixture sut = new Fixture();
        sut.Register<object, object, AbstractType>((obj1, obj2) => new ConcreteType(obj1, obj2));
        // Act
        AbstractType result = sut.Create<AbstractType>();
        // Assert
        Assert.NotNull(result.Property1);
        Assert.NotNull(result.Property2);
    }

    [Fact]
    public void CreateAnonymousWillUseRegisteredMappingWithTripleParameters()
    {
        // Arrange
        Fixture sut = new Fixture();
        sut.Register<object, object, object, AbstractType>((obj1, obj2, obj3) => new ConcreteType(obj1, obj2, obj3));
        // Act
        AbstractType result = sut.Create<AbstractType>();
        // Assert
        Assert.NotNull(result.Property1);
        Assert.NotNull(result.Property2);
        Assert.NotNull(result.Property3);
    }

    [Fact]
    public void CreateAnonymousWillUseRegisteredMappingWithQuadrupleParameters()
    {
        // Arrange
        Fixture sut = new Fixture();
        sut.Register<object, object, object, object, AbstractType>((obj1, obj2, obj3, obj4) => new ConcreteType(obj1, obj2, obj3, obj4));
        // Act
        AbstractType result = sut.Create<AbstractType>();
        // Assert
        Assert.NotNull(result.Property1);
        Assert.NotNull(result.Property2);
        Assert.NotNull(result.Property3);
        Assert.NotNull(result.Property4);
    }

    [Fact]
    public void CustomizeInstanceWillReturnFactory()
    {
        // Arrange
        Fixture sut = new Fixture();
        // Act
        var result = sut.Build<object>();
        // Assert
        Assert.NotNull(result);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void InjectDoesNotModifyAutoProperties(bool expected)
    {
        // Arrange
        var fixture = new Fixture();
        fixture.OmitAutoProperties = expected;
        // Act
        fixture.Inject("dummy");
        // Assert
        Assert.Equal(expected, fixture.OmitAutoProperties);
    }

    [Fact]
    public void CustomizationsContainStableFiniteSequenceRelayByDefault()
    {
        var sut = new Fixture();
        Assert.True(
            sut.Customizations.OfType<StableFiniteSequenceRelay>().Any(),
            "Stable finite sequence relay not found.");
    }

    [Theory]
    [InlineData(typeof(EnumeratorRelay))]
    [InlineData(typeof(EnumerableRelay))]
    public void ResidueCollectorsContainEnumerableRelayByDefault(
        Type relayType)
    {
        var sut = new Fixture();
        Assert.Contains(
            sut.ResidueCollectors,
            b => relayType.IsInstanceOfType(b));
    }

    [Theory]
    [InlineData(typeof(IList<>), typeof(List<>))]
    [InlineData(typeof(IReadOnlyList<>), typeof(ReadOnlyCollection<>))]
    [InlineData(typeof(ICollection<>), typeof(List<>))]
    [InlineData(typeof(IReadOnlyCollection<>), typeof(ReadOnlyCollection<>))]
    [InlineData(typeof(IDictionary<,>), typeof(Dictionary<,>))]
    [InlineData(typeof(IReadOnlyDictionary<,>), typeof(ReadOnlyDictionary<,>))]
#if NET5_0_OR_GREATER
    [InlineData(typeof(IReadOnlySet<>), typeof(HashSet<>))]
#endif
    public void ResidueCollectorsContainForwardsByDefault(Type from, Type to)
    {
        // Arrange
        var sut = new Fixture();
        // Act & assert
        Assert.Contains(
            sut.ResidueCollectors,
            b => b is TypeRelay tr && tr.From == from && tr.To == to);
    }

    [Theory]
    [InlineData(typeof(IList<object>), typeof(List<object>))]
    [InlineData(typeof(IReadOnlyList<object>), typeof(ReadOnlyCollection<object>))]
    [InlineData(typeof(ICollection<object>), typeof(List<object>))]
    [InlineData(typeof(IReadOnlyCollection<object>), typeof(ReadOnlyCollection<object>))]
    [InlineData(typeof(IDictionary<string, object>), typeof(Dictionary<string, object>))]
    [InlineData(typeof(IReadOnlyDictionary<string, object>), typeof(ReadOnlyDictionary<string, object>))]
#if NET5_0_OR_GREATER
    [InlineData(typeof(IReadOnlySet<object>), typeof(HashSet<object>))]
#endif
    public void DefaultForwardsAreUsedWhenTypeIsRequested(Type request, Type expected)
    {
        // Arrange
        var sut = new Fixture();
        var context = new SpecimenContext(sut);

        // Act
        var actual = context.Resolve(request);

        // Assert
        Assert.IsType(expected, actual);
    }

    [Theory]
    [InlineData(typeof(List<>), typeof(EnumerableFavoringConstructorQuery))]
    [InlineData(typeof(HashSet<>), typeof(EnumerableFavoringConstructorQuery))]
    [InlineData(typeof(Collection<>), typeof(ListFavoringConstructorQuery))]
    [InlineData(typeof(ObservableCollection<>), typeof(EnumerableFavoringConstructorQuery))]
    public void CustomizationsContainBuilderForProperConcreteMultipleTypeByDefault(
        Type matchingType,
        Type queryType)
    {
        var sut = new Fixture();
        Assert.Contains(
            sut.Customizations,
            b => b is FilteringSpecimenBuilder fsb
                 && fsb.Specification is ExactTypeSpecification ets && ets.TargetType == matchingType
                 && fsb.Builder is MethodInvoker mi && mi.Query.GetType() == queryType);
    }

    [Theory]
    [InlineData(typeof(Dictionary<,>), typeof(ModestConstructorQuery))]
    [InlineData(typeof(SortedDictionary<,>), typeof(ModestConstructorQuery))]
    [InlineData(typeof(SortedList<,>), typeof(ModestConstructorQuery))]
    public void CustomizationsContainBuilderForConcreteDictionariesByDefault(
        Type matchingType,
        Type queryType)
    {
        var sut = new Fixture();
        Assert.Contains(
            sut.Customizations,
            b => b is FilteringSpecimenBuilder fsb
                 && fsb.Specification is ExactTypeSpecification ets && ets.TargetType == matchingType
                 && fsb.Builder is Postprocessor pp && pp.Command is DictionaryFiller
                 && pp.Builder is MethodInvoker mi && mi.Query.GetType() == queryType);
    }

    [Fact]
    public void SutIsSequenceOfSpecimenBuilders()
    {
        var sut = new Fixture();
        Assert.IsAssignableFrom<IEnumerable<ISpecimenBuilder>>(sut);
    }

    [Fact]
    public void SutYieldsSomething()
    {
        var sut = new Fixture();

        Assert.True(sut.Any());
        Assert.NotEmpty(sut);
    }
}
