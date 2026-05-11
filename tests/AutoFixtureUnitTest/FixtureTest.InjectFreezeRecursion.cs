using System;
using System.Collections.Generic;
using System.Linq;
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
    public void InjectWillCauseSutToReturnInstanceWhenRequested()
    {
        // Arrange
        var expectedResult = new PropertyHolder<object>();
        var sut = new Fixture();
        sut.Inject(expectedResult);
        // Act
        var result = sut.Create<PropertyHolder<object>>();
        // Assert
        Assert.Equal<PropertyHolder<object>>(expectedResult, result);
    }

    [Fact]
    public void InjectWillCauseSutToReturnInstanceWithoutAutoPropertiesWhenRequested()
    {
        // Arrange
        var item = new PropertyHolder<object>();
        item.Property = null;

        var sut = new Fixture();
        sut.Inject(item);
        // Act
        var result = sut.Create<PropertyHolder<object>>();
        // Assert
        Assert.Null(result.Property);
    }

    [Fact]
    public void CreateAnonymousWillInvokeResidueCollector()
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
        sut.Create<PropertyHolder<AbstractType>>();
        // Assert
        Assert.True(resolveWasInvoked, "Resolver");
    }

    [Fact]
    public void CreateAnonymousOnUnregisteredAbstractionWillInvokeResidueCollectorWithCorrectType()
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
        sut.Create<PropertyHolder<AbstractType>>();
        // Assert (done by callback)
    }

    [Fact]
    public void CreateAnonymousOnUnregisteredAbstractionWillReturnInstanceFromResidueCollector()
    {
        // Arrange
        var expectedValue = new ConcreteType();

        var residueCollector = new DelegatingSpecimenBuilder();
        residueCollector.OnCreate = (r, c) => expectedValue;

        var sut = new Fixture();
        sut.ResidueCollectors.Add(residueCollector);
        // Act
        var result = sut.Create<PropertyHolder<AbstractType>>().Property;
        // Assert
        Assert.Equal<AbstractType>(expectedValue, result);
    }

    [Fact]
    public void FreezeWillCauseCreateAnonymousToKeepReturningTheFrozenInstance()
    {
        // Arrange
        var sut = new Fixture();
        var expectedResult = sut.Freeze<Guid>();
        // Act
        var result = sut.Create<Guid>();
        // Assert
        Assert.Equal<Guid>(expectedResult, result);
    }

    [Fact]
    public void FreezeWillCauseFixtureToKeepReturningTheFrozenInstanceEvenAsPropertyOfOtherType()
    {
        // Arrange
        var sut = new Fixture();
        var expectedResult = sut.Freeze<DateTime>();
        // Act
        var result = sut.Create<PropertyHolder<DateTime>>().Property;
        // Assert
        Assert.Equal<DateTime>(expectedResult, result);
    }

    [Fact]
    public void FreezeWithNullTransformationThrows()
    {
        // Arrange
        var sut = new Fixture();
        // Act & assert
        Assert.Throws<ArgumentNullException>(() =>
            sut.Freeze((Func<ICustomizationComposer<object>, ISpecimenBuilder>)null));
    }

    [Fact]
    public void FreezeBuiltInstanceWillCauseFixtureToKeepReturningTheFrozenInstance()
    {
        // Arrange
        var sut = new Fixture();
        var frozen = sut.Freeze<DoublePropertyHolder<DateTime, Guid>>(ob => ob.OmitAutoProperties().With(x => x.Property1));
        // Act
        var result = sut.Create<DoublePropertyHolder<DateTime, Guid>>();
        // Assert
        Assert.Equal(frozen.Property1, result.Property1);
        Assert.Equal(frozen.Property2, result.Property2);
    }

    [Fact]
    public void CreateManyWithDoCustomizationWillReturnCorrectResult()
    {
        // Arrange
        var sut = new Fixture();
        sut.Customize<List<string>>(ob => ob.Do(sut.AddManyTo).OmitAutoProperties());
        // Act
        var result = sut.CreateMany<List<string>>();
        // Assert
        Assert.True(result.All(l => l.Count == sut.RepeatCount), "Customize/Do/CreateMany");
    }

    [Fact]
    public void OmitAutoPropertiesFollowedByOptInWillNotSetOtherProperties()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        var result = sut.Build<DoublePropertyHolder<object, object>>()
            .OmitAutoProperties()
            .With(x => x.Property1)
            .Create();
        // Assert
        Assert.Null(result.Property2);
    }

    [Fact]
    public void OmitAutoPropertiesFollowedByTwoOptInsWillNotSetAnyOtherProperties()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        var result = sut.Build<TriplePropertyHolder<int, int, object>>()
            .OmitAutoProperties()
            .With(x => x.Property1, 42)
            .With(x => x.Property2, 1337)
            .Create();
        // Assert
        Assert.Equal(42, result.Property1);
        Assert.Equal(1337, result.Property2);
        Assert.Null(result.Property3);
    }

    [Fact]
    public void WithTwoOptInsFollowedByOmitAutoPropertiesWillNotSetAnyOtherProperties()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        var result = sut.Build<TriplePropertyHolder<int, int, object>>()
            .With(x => x.Property1, 42)
            .With(x => x.Property2, 1337)
            .OmitAutoProperties()
            .Create();
        // Assert
        Assert.Equal(42, result.Property1);
        Assert.Equal(1337, result.Property2);
        Assert.Null(result.Property3);
    }

    [Fact]
    public void CreateAnonymousWillThrowOnReferenceRecursionPoint()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        Assert.ThrowsAny<ObjectCreationException>(() =>
            sut.Create<RecursionTestObjectWithReferenceOutA>());
    }

    [Fact]
    public void CreateAnonymousWillThrowOnConstructorRecursionPoint()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        Assert.ThrowsAny<ObjectCreationException>(() =>
            sut.Create<RecursionTestObjectWithConstructorReferenceOutA>());
    }

    [Fact]
    public void BuildWithThrowingRecursionHandlerWillThrowOnReferenceRecursionPoint()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        Assert.ThrowsAny<ObjectCreationException>(() =>
            new SpecimenContext(
                    new RecursionGuard(
                        sut.Build<RecursionTestObjectWithReferenceOutA>(),
                        new ThrowingRecursionHandler()))
                .Create<RecursionTestObjectWithReferenceOutA>());
    }

    [Fact]
    public void BuildWithThrowingRecursionHandlerWillThrowOnConstructorRecursionPoint()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        Assert.ThrowsAny<ObjectCreationException>(() =>
            new SpecimenContext(
                    new RecursionGuard(
                        sut.Build<RecursionTestObjectWithConstructorReferenceOutA>(),
                        new ThrowingRecursionHandler()))
                .Create<RecursionTestObjectWithConstructorReferenceOutA>());
    }

    [Fact]
    public void BuildWithNullRecursionHandlerWillCreateNullOnRecursionPoint()
    {
        // Arrange
        var sut = new Fixture();
        // Act
        var result = new SpecimenContext(
                new RecursionGuard(
                    sut.Build<RecursionTestObjectWithConstructorReferenceOutA>(),
                    new NullRecursionHandler()))
            .Create<RecursionTestObjectWithConstructorReferenceOutA>();
        // Assert
        Assert.Null(result.ReferenceToB.ReferenceToA);
    }

    [Fact]
    public void BuildWithOmitRecursionGuardWillOmitPropertyOnRecursionPoint()
    {
        // Arrange
        var sut = new Fixture();
        sut.Behaviors.Clear();
        sut.Behaviors.Add(new OmitOnRecursionBehavior());
        // Act
        var actual = sut.Create<RecursionTestObjectWithReferenceOutA>();
        // Assert
        Assert.Null(actual.ReferenceToB.ReferenceToA);
    }

    [Fact]
    public void CreateAnonymousOnRegisteredInstanceWillReturnInstanceWithoutAutoProperties()
    {
        // Arrange
        var item = new PropertyHolder<string>();
        var sut = new Fixture();
        // Act
        sut.Inject(item);
        // Assert
        var result = sut.Create<PropertyHolder<string>>();
        Assert.Null(result.Property);
    }

    [Fact]
    public void CreateAnonymousOnRegisteredParameterlessFuncWillReturnInstanceWithoutAutoProperties()
    {
        // Arrange
        var item = new PropertyHolder<string>();
        var sut = new Fixture();
        // Act
        sut.Register(() => item);
        // Assert
        var result = sut.Create<PropertyHolder<string>>();
        Assert.Null(result.Property);
    }

    [Fact]
    public void CreateAnonymousOnRegisteredSingleParameterFuncWillReturnInstanceWithoutAutoProperties()
    {
        // Arrange
        var item = new PropertyHolder<string>();
        var sut = new Fixture();
        // Act
        sut.Register((object obj) => item);
        // Assert
        var result = sut.Create<PropertyHolder<string>>();
        Assert.Null(result.Property);
    }

    [Fact]
    public void CreateAnonymousOnRegisteredDoubleParameterFuncWillReturnInstanceWithoutAutoProperties()
    {
        // Arrange
        var item = new PropertyHolder<string>();
        var sut = new Fixture();
        // Act
        sut.Register((object obj1, object obj2) => item);
        // Assert
        var result = sut.Create<PropertyHolder<string>>();
        Assert.Null(result.Property);
    }

    [Fact]
    public void CreateAnonymousOnRegisteredTripleParameterFuncWillReturnInstanceWithoutAutoProperties()
    {
        // Arrange
        var item = new PropertyHolder<string>();
        var sut = new Fixture();
        // Act
        sut.Register((object obj1, object obj2, object obj3) => item);
        // Assert
        var result = sut.Create<PropertyHolder<string>>();
        Assert.Null(result.Property);
    }

    [Fact]
    public void CreateAnonymousOnRegisteredQuadrupleParameterFuncWillReturnInstanceWithoutAutoProperties()
    {
        // Arrange
        var item = new PropertyHolder<string>();
        var sut = new Fixture();
        // Act
        sut.Register((object obj1, object obj2, object obj3, object obj4) => item);
        // Assert
        var result = sut.Create<PropertyHolder<string>>();
        Assert.Null(result.Property);
    }

    [Fact]
    public void CreateAnonymousWithOmitAutoPropertiesWillNotAssignProperty()
    {
        // Arrange
        Fixture sut = new Fixture() { OmitAutoProperties = true };
        // Act
        PropertyHolder<string> result = sut.Create<PropertyHolder<string>>();
        // Assert
        Assert.Null(result.Property);
    }

    [Fact]
    public void CustomizeInstanceWithOmitAutoPropertiesWillReturnFactoryWithOmitAutoProperties()
    {
        // Arrange
        var sut = new Fixture() { OmitAutoProperties = true };
        // Act
        var builder = sut.Build<PropertyHolder<object>>();
        PropertyHolder<object> result = builder.Create();
        // Assert
        Assert.Null(result.Property);
    }

    [Fact]
    public void FreezedFirstCallToCreateAnonymousWithOmitAutoPropertiesWillNotAssignProperty()
    {
        // Arrange
        var sut = new Fixture() { OmitAutoProperties = true };
        // Act
        var expectedResult = sut.Freeze<PropertyHolder<string>>();
        // Assert
        Assert.Null(expectedResult.Property);
    }

    [Fact]
    public void CustomizedBuilderCreateAnonymousWithOmitAutoPropertiesWillNotAssignProperty()
    {
        // Arrange
        var sut = new Fixture() { OmitAutoProperties = true };
        // Act
        sut.Customize<PropertyHolder<string>>(x => x);
        var expectedResult = sut.Create<PropertyHolder<string>>();
        // Assert
        Assert.Null(expectedResult.Property);
    }

    [Fact]
    public void CustomizedOverrideOfOmitAutoPropertiesWillAssignProperty()
    {
        // Arrange
        var sut = new Fixture() { OmitAutoProperties = true };
        // Act
        sut.Customize<PropertyHolder<string>>(x => x.WithAutoProperties());
        var expectedResult = sut.Create<PropertyHolder<string>>();
        // Assert
        Assert.NotNull(expectedResult.Property);
    }

    [Fact]
    public void DefaultOmitAutoPropertiesIsFalse()
    {
        // Arrange
        Fixture sut = new Fixture();
        // Act
        bool result = sut.OmitAutoProperties;
        // Assert
        Assert.False(result, "OmitAutoProperties");
    }

    // Supporting http://autofixture.codeplex.com/discussions/262288 Breaking this test might not be considered a breaking change
    [Fact]
    public void RefreezeHack()
    {
        // Arrange
        var fixture = new Fixture();
        var version2 = fixture.Create<Version>();
        var version1 = fixture.Freeze<Version>();
        // Act
        fixture.Inject(version2);
        var actual = fixture.Create<Version>();
        // Assert
        Assert.Equal(version2, actual);
    }

    // Supporting http://autofixture.codeplex.com/discussions/262288 Breaking this test might not be considered a breaking change
    [Fact]
    public void UnfreezeHack()
    {
        // Arrange
        var fixture = new Fixture();
        var snapshot = fixture.Customizations.ToList();
        var anonymousVersion = fixture.Freeze<Version>();
        var freezer = fixture.Customizations.Except(snapshot).Single();
        // Act
        fixture.Customizations.Remove(freezer);
        var expected = fixture.Freeze<Version>();
        var actual = fixture.Create<Version>();
        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CreateSmallRecursiveSequenceGraph()
    {
        // Arrange
        var fixture = new Fixture();
        fixture.Behaviors
            .OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior(2));
        // Act
        var actual = fixture.Create<RecursiveSequenceNode>();
        // Assert
        Assert.NotEmpty(actual);
        Assert.True(actual.All(n => !n.Any()));
    }

    [Fact]
    public void CreateSequenceOfSmallRecursiveSequenceGraphs()
    {
        // Arrange
        var fixture = new Fixture();
        fixture.Behaviors
            .OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));
        fixture.Customizations.Add(
            new OmitEnumerableParameterRequestRelay());
        // Act
        var actual = fixture.Create<IEnumerable<RecursiveSequenceNode>>();
        // Assert
        Assert.NotEmpty(actual);
        Assert.True(actual.All(n => !n.Any()));
    }

    [Fact]
    public void CreateArrayOfSmallRecursiveSequenceGraphs()
    {
        // Arrange
        var fixture = new Fixture();
        fixture.Behaviors
            .OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));
        fixture.Customizations.Add(
            new OmitEnumerableParameterRequestRelay());
        // Act
        var actual = fixture.Create<RecursiveSequenceNode[]>();
        // Assert
        Assert.NotEmpty(actual);
        Assert.True(actual.All(n => !n.Any()));
    }

    [Fact]
    public void CreateManySmallRecursiveSequenceGraphs()
    {
        // Arrange
        var fixture = new Fixture();
        fixture.Behaviors
            .OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));
        fixture.Customizations.Add(
            new OmitEnumerableParameterRequestRelay());
        // Act
        var actual = fixture.CreateMany<RecursiveSequenceNode>();
        // Assert
        Assert.NotEmpty(actual);
        Assert.True(actual.All(n => !n.Any()));
    }

    private class RecursiveSequenceNode : IEnumerable<RecursiveSequenceNode>
    {
        private readonly IEnumerable<RecursiveSequenceNode> _nodes;

        public RecursiveSequenceNode(IEnumerable<RecursiveSequenceNode> nodes)
        {
            _nodes = nodes;
        }

        public IEnumerator<RecursiveSequenceNode> GetEnumerator()
        {
            return _nodes.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    [Fact]
    public void CreateSmallRecursiveArrayGraph()
    {
        // Arrange
        var fixture = new Fixture();
        fixture.Behaviors
            .OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior(2));
        // Act
        var actual = fixture.Create<RecursiveArrayNode>();
        // Assert
        Assert.NotEmpty(actual);
        Assert.True(actual.All(n => !n.Any()));
    }

    [Fact]
    public void CreateSequenceOfSmallRecursiveArrayGraphs()
    {
        // Arrange
        var fixture = new Fixture();
        fixture.Behaviors
            .OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));
        fixture.Customizations.Add(
            new OmitArrayParameterRequestRelay());
        // Act
        var actual = fixture.Create<IEnumerable<RecursiveArrayNode>>();
        // Assert
        Assert.NotEmpty(actual);
        Assert.True(actual.All(n => !n.Any()));
    }

    [Fact]
    public void CreateArrayOfSmallRecursiveArrayGraphs()
    {
        // Arrange
        var fixture = new Fixture();
        fixture.Behaviors
            .OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));
        fixture.Customizations.Add(
            new OmitArrayParameterRequestRelay());
        // Act
        var actual = fixture.Create<RecursiveArrayNode[]>();
        // Assert
        Assert.NotEmpty(actual);
        Assert.True(actual.All(n => !n.Any()));
    }

    [Fact]
    public void CreateManySmallRecursiveArrayGraphs()
    {
        // Arrange
        var fixture = new Fixture();
        fixture.Behaviors
            .OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));
        fixture.Customizations.Add(
            new OmitArrayParameterRequestRelay());
        // Act
        var actual = fixture.CreateMany<RecursiveArrayNode>();
        // Assert
        Assert.NotEmpty(actual);
        Assert.True(actual.All(n => !n.Any()));
    }

    private class RecursiveArrayNode : IEnumerable<RecursiveArrayNode>
    {
        private readonly RecursiveArrayNode[] _nodes;

        public RecursiveArrayNode(RecursiveArrayNode[] nodes)
        {
            _nodes = nodes;
        }

        public IEnumerator<RecursiveArrayNode> GetEnumerator()
        {
            return _nodes.AsEnumerable().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    private class RecursionTestObjectWithReferenceOutA
    {
        public RecursionTestObjectWithReferenceOutB ReferenceToB
        {
            get;
            set;
        }
    }

    private class RecursionTestObjectWithReferenceOutB
    {
        public RecursionTestObjectWithReferenceOutA ReferenceToA
        {
            get;
            set;
        }
    }

    private class RecursionTestObjectWithConstructorReferenceOutA
    {
        public RecursionTestObjectWithConstructorReferenceOutB ReferenceToB
        {
            get;
            private set;
        }

        public RecursionTestObjectWithConstructorReferenceOutA(RecursionTestObjectWithConstructorReferenceOutB b)
        {
            ReferenceToB = b;
        }
    }

    private class RecursionTestObjectWithConstructorReferenceOutB
    {
        public RecursionTestObjectWithConstructorReferenceOutA ReferenceToA
        {
            get;
            private set;
        }

        public RecursionTestObjectWithConstructorReferenceOutB(RecursionTestObjectWithConstructorReferenceOutA a)
        {
            ReferenceToA = a;
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void OmitAutoProperitesIsChangedAfterAssignment(bool value)
    {
        // Arrange
        var sut = new Fixture
        {
            OmitAutoProperties = !value
        };

        // Act
        sut.OmitAutoProperties = value;

        // Assert
        Assert.Equal(value, sut.OmitAutoProperties);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Optimization_GraphIsNotMutatedWhenActualOmitAutoPropertiesValueIsNotChanged(bool initialValue)
    {
        // Arrange
        var sut = new Fixture();
        sut.OmitAutoProperties = initialValue;

        var oldBehaviors = sut.Behaviors;
        var oldCustomizations = sut.Customizations;
        var oldResidueCollectors = sut.ResidueCollectors;

        // Act
        sut.OmitAutoProperties = initialValue;

        // Assert
        Assert.Same(oldBehaviors, sut.Behaviors);
        Assert.Same(oldCustomizations, sut.Customizations);
        Assert.Same(oldResidueCollectors, sut.ResidueCollectors);
    }
}
