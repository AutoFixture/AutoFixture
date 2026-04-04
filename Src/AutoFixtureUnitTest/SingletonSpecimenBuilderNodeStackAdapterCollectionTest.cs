using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest;

public class SingletonSpecimenBuilderNodeStackAdapterCollectionTest
{
    private readonly ISpecimenBuilderNode _graph;
    private readonly SingletonSpecimenBuilderNodeStackAdapterCollection _sut;

    public SingletonSpecimenBuilderNodeStackAdapterCollectionTest()
    {
        _graph = new MarkerNode(
            new CompositeSpecimenBuilder(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    new DelegatingSpecimenBuilder(),
                    new DelegatingSpecimenBuilder()),
                new CompositeSpecimenBuilder(new DelegatingSpecimenBuilder(),
                    new DelegatingSpecimenBuilder(),
                    new DelegatingSpecimenBuilder()),
                new CompositeSpecimenBuilder(new DelegatingSpecimenBuilder(),
                    new DelegatingSpecimenBuilder(),
                    new DelegatingSpecimenBuilder())));
        _sut = new SingletonSpecimenBuilderNodeStackAdapterCollection(_graph, n => n is MarkerNode);
    }

    [Fact]
    public void SutIsSpecimenBuilderTransformationList()
    {
        Assert.IsAssignableFrom<IList<ISpecimenBuilderTransformation>>(_sut);
    }

    [Fact]
    public void SutIsCollection()
    {
        Assert.IsAssignableFrom<Collection<ISpecimenBuilderTransformation>>(_sut);
    }

    [Fact]
    public void InitialGraphIsCorrect()
    {
        // Arrange
        // Act
        ISpecimenBuilderNode actual = _sut.Graph;
        // Assert
        Assert.Equal(_graph, actual);
    }

    [Fact]
    public void InsertRaisesGraphChanged()
    {
        // Arrange
        var verified = false;
        _sut.GraphChanged += (s, e) => verified = s != null && e != null && e.Graph == _sut.Graph;
        // Act
        var dummyIndex = 0;
        var dummyItem = new DelegatingSpecimenBuilderTransformation();
        _sut.Insert(dummyIndex, dummyItem);
        // Assert
        Assert.True(verified);
    }

    [Fact]
    public void RemoveAtRaisesGraphChanged()
    {
        // Arrange
        _sut.Add(new DelegatingSpecimenBuilderTransformation());
        var verified = false;
        _sut.GraphChanged += (s, e) => verified = s != null && e != null && e.Graph == _sut.Graph;
        // Act
        var dummyIndex = 0;
        _sut.RemoveAt(dummyIndex);
        // Assert
        Assert.True(verified);
    }

    [Fact]
    public void SetItemRaisesGraphChanged()
    {
        // Arrange
        _sut.Add(new DelegatingSpecimenBuilderTransformation());
        var verified = false;
        _sut.GraphChanged += (s, e) => verified = s != null && e != null && e.Graph == _sut.Graph;
        // Act
        var dummyIndex = 0;
        var dummyItem = new DelegatingSpecimenBuilderTransformation();
        _sut[dummyIndex] = dummyItem;
        // Assert
        Assert.True(verified);
    }

    [Fact]
    public void AddRaisesGraphChanged()
    {
        // Arrange
        var verified = false;
        _sut.GraphChanged += (s, e) => verified = s != null && e != null && e.Graph == _sut.Graph;
        // Act
        var dummyItem = new DelegatingSpecimenBuilderTransformation();
        _sut.Add(dummyItem);
        // Assert
        Assert.True(verified);
    }

    [Fact]
    public void ClearNonEmptyCollectionRaisesGraphChanged()
    {
        // Arrange
        _sut.Add(new DelegatingSpecimenBuilderTransformation());
        var verified = false;
        _sut.GraphChanged += (s, e) => verified = s != null && e != null && e.Graph == _sut.Graph;
        // Act
        _sut.Clear();
        // Assert
        Assert.True(verified);
    }

    [Fact]
    public void ClearEmptyCollectionDoesNotRaiseGraphChanged()
    {
        // Arrange
        _sut.Clear();
        var invoked = false;
        _sut.GraphChanged += (s, e) => invoked = true;
        // Act
        _sut.Clear();
        // Assert
        Assert.False(invoked);
    }

    [Fact]
    public void RemoveContainedItemRaisesGraphChanged()
    {
        // Arrange
        var item = new DelegatingSpecimenBuilderTransformation();
        _sut.Add(item);

        var verified = false;
        _sut.GraphChanged += (s, e) => verified = s != null && e != null && e.Graph == _sut.Graph;
        // Act
        _sut.Remove(item);
        // Assert
        Assert.True(verified);
    }

    [Fact]
    public void RemoveUncontainedItemDoesNotRaiseGraphChanged()
    {
        // Arrange
        var item = new DelegatingSpecimenBuilderTransformation();
        var invoked = false;
        _sut.GraphChanged += (s, e) => invoked = true;
        // Act
        _sut.Remove(item);
        // Assert
        Assert.False(invoked);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void InsertItemCorrectlyChangesGraph(int index)
    {
        // Arrange
        _sut.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode("A", b) });
        _sut.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode("B", b) });
        _sut.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode("C", b) });
        // Act
        var item = new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode(index, b) };
        _sut.Insert(index, item);
        // Assert
        var expected = _sut.Aggregate(
            _graph,
            (b, t) => (ISpecimenBuilderNode)t.Transform(b));

        Assert.True(expected.GraphEquals(_sut.Graph,
            new TaggedNodeComparer(new TrueComparer<ISpecimenBuilder>())));
    }

    [Fact]
    public void ClearCorrectlyChangesGraph()
    {
        // Arrange
        _sut.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode("A", b) });
        _sut.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode("B", b) });
        _sut.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode("C", b) });
        // Act
        _sut.Clear();
        // Assert
        Assert.True(_graph.GraphEquals(_sut.Graph,
            new TrueComparer<ISpecimenBuilder>()));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void RemoveAtCorrectlyChangesGraph(int index)
    {
        // Arrange
        _sut.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode("A", b) });
        _sut.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode("B", b) });
        _sut.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode("C", b) });
        // Act
        _sut.RemoveAt(index);
        // Assert
        var expected = _sut.Aggregate(
            _graph,
            (b, t) => (ISpecimenBuilderNode)t.Transform(b));

        Assert.True(expected.GraphEquals(_sut.Graph,
            new TaggedNodeComparer(new TrueComparer<ISpecimenBuilder>())));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void SetItemCorrectlyChangesGraph(int index)
    {
        // Arrange
        _sut.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode("A", b) });
        _sut.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode("B", b) });
        _sut.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode("C", b) });
        // Act
        var item = new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode(index, b) };
        _sut[index] = item;
        // Assert
        var expected = _sut.Aggregate(
            _graph,
            (b, t) => (ISpecimenBuilderNode)t.Transform(b));

        Assert.True(expected.GraphEquals(_sut.Graph,
            new TaggedNodeComparer(new TrueComparer<ISpecimenBuilder>())));
    }

    [Fact]
    public void SutContainsItemsFromConstructor()
    {
        // Arrange
        var x = new DelegatingSpecimenBuilderTransformation();
        var y = new DelegatingSpecimenBuilderTransformation();
        var z = new DelegatingSpecimenBuilderTransformation();
        // Act
        var s = new SingletonSpecimenBuilderNodeStackAdapterCollection(
            _graph, n => n is MarkerNode, x, y, z);
        // Assert
        Assert.True(new[] { x, y, z }.SequenceEqual(s));
    }

    [Fact]
    public void ConstructWithNullGraphThrows()
    {
        // Arrange
        Func<ISpecimenBuilderNode, bool> dummyPredicate = n => false;
        var dummyTransformations = new ISpecimenBuilderTransformation[0];
        // Act & assert
        Assert.Throws<ArgumentNullException>(() =>
            new SingletonSpecimenBuilderNodeStackAdapterCollection(
                null,
                dummyPredicate,
                dummyTransformations));
    }

    [Fact]
    public void ConstructWithNullPredicateThrows()
    {
        // Arrange
        var dummyGraph = new CompositeSpecimenBuilder();
        var dummyTransformations = new ISpecimenBuilderTransformation[0];
        // Act & assert
        Assert.Throws<ArgumentNullException>(() =>
            new SingletonSpecimenBuilderNodeStackAdapterCollection(
                dummyGraph,
                null,
                dummyTransformations));
    }

    [Fact]
    public void ConstructWithNullTransformationsThrows()
    {
        // Arrange
        var dummyGraph = new CompositeSpecimenBuilder();
        Func<ISpecimenBuilderNode, bool> dummyPredicate = n => false;
        // Act & assert
        Assert.Throws<ArgumentNullException>(() =>
            new SingletonSpecimenBuilderNodeStackAdapterCollection(
                dummyGraph,
                dummyPredicate,
                null));
    }
}