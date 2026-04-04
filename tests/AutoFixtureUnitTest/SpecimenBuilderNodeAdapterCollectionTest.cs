using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest;

public class SpecimenBuilderNodeAdapterCollectionTest
{
    private readonly ISpecimenBuilderNode _graph;
    private readonly SpecimenBuilderNodeAdapterCollection _sut;

    public SpecimenBuilderNodeAdapterCollectionTest()
    {
        _graph = new CompositeSpecimenBuilder(
            new CompositeSpecimenBuilder(
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()),
            new MarkerNode(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    new DelegatingSpecimenBuilder(),
                    new DelegatingSpecimenBuilder())),
            new CompositeSpecimenBuilder(
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()));
        _sut = new SpecimenBuilderNodeAdapterCollection(_graph, s => s is MarkerNode);
    }

    [Fact]
    public void SutIsSpecimenBuilderList()
    {
        // Arrange
        // Act
        // Assert
        Assert.IsAssignableFrom<IList<ISpecimenBuilder>>(_sut);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void IndexOfReturnsCorrectResult(int expected)
    {
        // Arrange
        var item = FindMarkedNode().ElementAt(expected);
        // Act
        var actual = _sut.IndexOf(item);
        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void IndexOfReturnsCorrectResultWhenItemIsNotInNode()
    {
        // Arrange
        var item = new DelegatingSpecimenBuilder();
        // Act
        var actual = _sut.IndexOf(item);
        // Assert
        Assert.Equal(-1, actual);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void InsertsCorrectlyInsertsItem(int expected)
    {
        // Arrange
        var item = new DelegatingSpecimenBuilder();
        // Act
        _sut.Insert(expected, item);
        // Assert
        var actual = _sut.IndexOf(item);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void ContainsReturnsTrueForContainedItem(int index)
    {
        // Arrange
        var item = FindMarkedNode().ElementAt(index);
        // Act
        var actual = _sut.Contains(item);
        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void ContainsReturnFalseForUncontainedItem()
    {
        // Arrange
        var uncontainedItem = new DelegatingSpecimenBuilder();
        // Act
        var actual = _sut.Contains(uncontainedItem);
        // Assert
        Assert.False(actual);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void RemoveAtCorrectlyRemovesItem(int index)
    {
        // Arrange
        var itemToBeRemoved = FindMarkedNode().ElementAt(index);
        // Act
        _sut.RemoveAt(index);
        // Assert
        Assert.DoesNotContain(itemToBeRemoved, _sut);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void GetItemReturnsCorrectResult(int index)
    {
        // Arrange
        var expected = FindMarkedNode().ElementAt(index);
        // Act
        var actual = _sut[index];
        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetItemForIncorrectIndexThrows()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            _sut[1337]);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void SetItemCorrectlyAddsItem(int expected)
    {
        // Arrange
        var item = new DelegatingSpecimenBuilder();
        // Act
        _sut[expected] = item;
        // Assert
        Assert.Equal(expected, _sut.IndexOf(item));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void SetItemCorrectlyRemovesExistingItem(int index)
    {
        // Arrange
        var itemToReplace = FindMarkedNode().ElementAt(index);
        // Act
        _sut[index] = new DelegatingSpecimenBuilder();
        // Assert
        Assert.DoesNotContain(itemToReplace, _sut);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(42)]
    [InlineData(3)]
    public void SetItemForIncorrectIndexThrows(int invalidIndex)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            _sut[invalidIndex] = new DelegatingSpecimenBuilder());
    }

    [Fact]
    public void SutYieldsCorrectItems()
    {
        var expected = (ISpecimenBuilderNode)_graph
            .OfType<MarkerNode>().Single().Single();
        Assert.True(expected.SequenceEqual(_sut));
        Assert.True(expected.Cast<object>().SequenceEqual(((System.Collections.IEnumerable)_sut).Cast<object>()));
    }

    [Fact]
    public void CountReturnsCorrectResult()
    {
        // Arrange
        // Act
        var actual = _sut.Count;
        // Assert
        var expected = FindMarkedNode().Count();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ClearRemovesAllItems()
    {
        // Arrange
        // Act
        _sut.Clear();
        // Assert
        Assert.Empty(_sut);
    }

    [Fact]
    public void AddAddsItemToEndOfNode()
    {
        // Arrange
        var item = new DelegatingSpecimenBuilder();
        var expected = FindMarkedNode().Concat(new[] { item });
        // Act
        _sut.Add(item);
        // Assert
        Assert.True(expected.SequenceEqual(_sut));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void CopyToCorrectlyCopiesItems(int index)
    {
        // Arrange
        var expected = FindMarkedNode().ToArray();
        var a = new ISpecimenBuilder[expected.Length + index];
        // Act
        _sut.CopyTo(a, index);
        // Assert
        Assert.True(expected.SequenceEqual(a.Skip(index)));
    }

    [Fact]
    public void IsReadOnlyReturnsCorrectResult()
    {
        Assert.False(_sut.IsReadOnly);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void RemoveCorrectlyRemovesItem(int index)
    {
        // Arrange
        var item = FindMarkedNode().ElementAt(index);
        // Act
        _sut.Remove(item);
        // Assert
        Assert.DoesNotContain(item, _sut);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void RemoveReturnsTrueForContainedItem(int index)
    {
        // Arrange
        var item = FindMarkedNode().ElementAt(index);
        // Act
        var actual = _sut.Remove(item);
        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void RemoveReturnsFalseForUncontainedItem()
    {
        // Arrange
        var uncontainedItem = new DelegatingSpecimenBuilder();
        // Act
        var actual = _sut.Remove(uncontainedItem);
        // Assert
        Assert.False(actual);
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
        _sut.GraphChanged += (s, e) => verified = s != null && e != null;
        // Act
        var dummyIndex = 1;
        var dummyItem = new DelegatingSpecimenBuilder();
        _sut.Insert(dummyIndex, dummyItem);
        // Assert
        Assert.True(verified);
    }

    [Fact]
    public void RemoveAtRaisesGraphChanged()
    {
        // Arrange
        var verified = false;
        _sut.GraphChanged += (s, e) => verified = s != null && e != null;
        // Act
        var dummyIndex = 1;
        _sut.RemoveAt(dummyIndex);
        // Assert
        Assert.True(verified);
    }

    [Fact]
    public void SetItemRaisesGraphChanged()
    {
        // Arrange
        var verified = false;
        _sut.GraphChanged += (s, e) => verified = s != null && e != null;
        // Act
        var dummyIndex = 1;
        var dummyItem = new DelegatingSpecimenBuilder();
        _sut[dummyIndex] = dummyItem;
        // Assert
        Assert.True(verified);
    }

    [Fact]
    public void AddRaisesGraphChanged()
    {
        // Arrange
        var verified = false;
        _sut.GraphChanged += (s, e) => verified = s != null && e != null;
        // Act
        var dummyItem = new DelegatingSpecimenBuilder();
        _sut.Add(dummyItem);
        // Assert
        Assert.True(verified);
    }

    [Fact]
    public void ClearRaisesGraphChanged()
    {
        // Arrange
        var verified = false;
        _sut.GraphChanged += (s, e) => verified = s != null && e != null;
        // Act
        _sut.Clear();
        // Assert
        Assert.True(verified);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void RemoveRaisesGraphChanged(int index)
    {
        // Arrange
        var verified = false;
        _sut.GraphChanged += (s, e) => verified = s != null && e != null;

        var item = FindMarkedNode().ElementAt(index);
        // Act
        _sut.Remove(item);
        // Assert
        Assert.True(verified);
    }

    private ISpecimenBuilderNode FindMarkedNode()
    {
        return (ISpecimenBuilderNode)_graph
            .OfType<MarkerNode>()
            .Single()
            .Single();
    }
}