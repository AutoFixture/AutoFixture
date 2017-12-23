using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class SpecimenBuilderNodeAdapterCollectionTest
    {
        private readonly ISpecimenBuilderNode graph;
        private readonly SpecimenBuilderNodeAdapterCollection sut;

        public SpecimenBuilderNodeAdapterCollectionTest()
        {
            this.graph = new CompositeSpecimenBuilder(
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
            this.sut = new SpecimenBuilderNodeAdapterCollection(this.graph, s => s is MarkerNode);
        }

        [Fact]
        public void SutIsSpecimenBuilderList()
        {
            // Arrange
            // Act
            // Assert
            Assert.IsAssignableFrom<IList<ISpecimenBuilder>>(this.sut);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void IndexOfReturnsCorrectResult(int expected)
        {
            // Arrange
            var item = this.FindMarkedNode().ElementAt(expected);
            // Act
            var actual = this.sut.IndexOf(item);
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IndexOfReturnsCorrectResultWhenItemIsNotInNode()
        {
            // Arrange
            var item = new DelegatingSpecimenBuilder();
            // Act
            var actual = this.sut.IndexOf(item);
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
            this.sut.Insert(expected, item);
            // Assert
            var actual = this.sut.IndexOf(item);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void ContainsReturnsTrueForContainedItem(int index)
        {
            // Arrange
            var item = this.FindMarkedNode().ElementAt(index);
            // Act
            var actual = this.sut.Contains(item);
            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void ContainsReturnFalseForUncontainedItem()
        {
            // Arrange
            var uncontainedItem = new DelegatingSpecimenBuilder();
            // Act
            var actual = this.sut.Contains(uncontainedItem);
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
            var itemToBeRemoved = this.FindMarkedNode().ElementAt(index);
            // Act
            this.sut.RemoveAt(index);
            // Assert
            Assert.DoesNotContain(itemToBeRemoved, this.sut);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void GetItemReturnsCorrectResult(int index)
        {
            // Arrange
            var expected = this.FindMarkedNode().ElementAt(index);
            // Act
            var actual = this.sut[index];
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetItemForIncorrectIndexThrows()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                this.sut[1337]);
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
            this.sut[expected] = item;
            // Assert
            Assert.Equal(expected, this.sut.IndexOf(item));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void SetItemCorrectlyRemovesExistingItem(int index)
        {
            // Arrange
            var itemToReplace = this.FindMarkedNode().ElementAt(index);
            // Act
            this.sut[index] = new DelegatingSpecimenBuilder();
            // Assert
            Assert.DoesNotContain(itemToReplace, this.sut);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(42)]
        [InlineData(3)]
        public void SetItemForIncorrectIndexThrows(int invalidIndex)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                this.sut[invalidIndex] = new DelegatingSpecimenBuilder());
        }

        [Fact]
        public void SutYieldsCorrectItems()
        {
            var expected = (ISpecimenBuilderNode)this.graph
                .OfType<MarkerNode>().Single().Single();
            Assert.True(expected.SequenceEqual(this.sut));
            Assert.True(expected.Cast<object>().SequenceEqual(((System.Collections.IEnumerable)this.sut).Cast<object>()));
        }

        [Fact]
        public void CountReturnsCorrectResult()
        {
            // Arrange
            // Act
            var actual = this.sut.Count;
            // Assert
            var expected = this.FindMarkedNode().Count();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ClearRemovesAllItems()
        {
            // Arrange
            // Act
            this.sut.Clear();
            // Assert
            Assert.Empty(this.sut);
        }

        [Fact]
        public void AddAddsItemToEndOfNode()
        {
            // Arrange
            var item = new DelegatingSpecimenBuilder();
            var expected = this.FindMarkedNode().Concat(new[] { item });
            // Act
            this.sut.Add(item);
            // Assert
            Assert.True(expected.SequenceEqual(this.sut));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void CopyToCorrectlyCopiesItems(int index)
        {
            // Arrange
            var expected = this.FindMarkedNode().ToArray();
            var a = new ISpecimenBuilder[expected.Length + index];
            // Act
            this.sut.CopyTo(a, index);
            // Assert
            Assert.True(expected.SequenceEqual(a.Skip(index)));
        }

        [Fact]
        public void IsReadOnlyReturnsCorrectResult()
        {
            Assert.False(this.sut.IsReadOnly);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void RemoveCorrectlyRemovesItem(int index)
        {
            // Arrange
            var item = this.FindMarkedNode().ElementAt(index);
            // Act
            this.sut.Remove(item);
            // Assert
            Assert.DoesNotContain(item, this.sut);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void RemoveReturnsTrueForContainedItem(int index)
        {
            // Arrange
            var item = this.FindMarkedNode().ElementAt(index);
            // Act
            var actual = this.sut.Remove(item);
            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void RemoveReturnsFalseForUncontainedItem()
        {
            // Arrange
            var uncontainedItem = new DelegatingSpecimenBuilder();
            // Act
            var actual = this.sut.Remove(uncontainedItem);
            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void InitialGraphIsCorrect()
        {
            // Arrange
            // Act
            ISpecimenBuilderNode actual = this.sut.Graph;
            // Assert
            Assert.Equal(this.graph, actual);
        }

        [Fact]
        public void InsertRaisesGraphChanged()
        {
            // Arrange
            var verified = false;
            this.sut.GraphChanged += (s, e) => verified = s != null && e != null;
            // Act
            var dummyIndex = 1;
            var dummyItem = new DelegatingSpecimenBuilder();
            this.sut.Insert(dummyIndex, dummyItem);
            // Assert
            Assert.True(verified);
        }

        [Fact]
        public void RemoveAtRaisesGraphChanged()
        {
            // Arrange
            var verified = false;
            this.sut.GraphChanged += (s, e) => verified = s != null && e != null;
            // Act
            var dummyIndex = 1;
            this.sut.RemoveAt(dummyIndex);
            // Assert
            Assert.True(verified);
        }

        [Fact]
        public void SetItemRaisesGraphChanged()
        {
            // Arrange
            var verified = false;
            this.sut.GraphChanged += (s, e) => verified = s != null && e != null;
            // Act
            var dummyIndex = 1;
            var dummyItem = new DelegatingSpecimenBuilder();
            this.sut[dummyIndex] = dummyItem;
            // Assert
            Assert.True(verified);
        }

        [Fact]
        public void AddRaisesGraphChanged()
        {
            // Arrange
            var verified = false;
            this.sut.GraphChanged += (s, e) => verified = s != null && e != null;
            // Act
            var dummyItem = new DelegatingSpecimenBuilder();
            this.sut.Add(dummyItem);
            // Assert
            Assert.True(verified);
        }

        [Fact]
        public void ClearRaisesGraphChanged()
        {
            // Arrange
            var verified = false;
            this.sut.GraphChanged += (s, e) => verified = s != null && e != null;
            // Act
            this.sut.Clear();
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
            this.sut.GraphChanged += (s, e) => verified = s != null && e != null;

            var item = this.FindMarkedNode().ElementAt(index);
            // Act
            this.sut.Remove(item);
            // Assert
            Assert.True(verified);
        }

        private ISpecimenBuilderNode FindMarkedNode()
        {
            return (ISpecimenBuilderNode)this.graph
                .OfType<MarkerNode>()
                .Single()
                .Single();
        }
    }
}
