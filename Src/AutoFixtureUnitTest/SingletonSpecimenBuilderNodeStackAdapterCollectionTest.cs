using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class SingletonSpecimenBuilderNodeStackAdapterCollectionTest
    {
        private readonly ISpecimenBuilderNode graph;
        private readonly SingletonSpecimenBuilderNodeStackAdapterCollection sut;

        public SingletonSpecimenBuilderNodeStackAdapterCollectionTest()
        {
            this.graph = new MarkerNode(
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
            this.sut = new SingletonSpecimenBuilderNodeStackAdapterCollection(this.graph, n => n is MarkerNode);
        }

        [Fact]
        public void SutIsSpecimenBuilderTransformationList()
        {
            Assert.IsAssignableFrom<IList<ISpecimenBuilderTransformation>>(this.sut);
        }

        [Fact]
        public void SutIsCollection()
        {
            Assert.IsAssignableFrom<Collection<ISpecimenBuilderTransformation>>(this.sut);
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
            this.sut.GraphChanged += (s, e) => verified = s != null && e != null && e.Graph == this.sut.Graph;
            // Act
            var dummyIndex = 0;
            var dummyItem = new DelegatingSpecimenBuilderTransformation();
            this.sut.Insert(dummyIndex, dummyItem);
            // Assert
            Assert.True(verified);
        }

        [Fact]
        public void RemoveAtRaisesGraphChanged()
        {
            // Arrange
            this.sut.Add(new DelegatingSpecimenBuilderTransformation());
            var verified = false;
            this.sut.GraphChanged += (s, e) => verified = s != null && e != null && e.Graph == this.sut.Graph;
            // Act
            var dummyIndex = 0;
            this.sut.RemoveAt(dummyIndex);
            // Assert
            Assert.True(verified);
        }

        [Fact]
        public void SetItemRaisesGraphChanged()
        {
            // Arrange
            this.sut.Add(new DelegatingSpecimenBuilderTransformation());
            var verified = false;
            this.sut.GraphChanged += (s, e) => verified = s != null && e != null && e.Graph == this.sut.Graph;
            // Act
            var dummyIndex = 0;
            var dummyItem = new DelegatingSpecimenBuilderTransformation();
            this.sut[dummyIndex] = dummyItem;
            // Assert
            Assert.True(verified);
        }

        [Fact]
        public void AddRaisesGraphChanged()
        {
            // Arrange
            var verified = false;
            this.sut.GraphChanged += (s, e) => verified = s != null && e != null && e.Graph == this.sut.Graph;
            // Act
            var dummyItem = new DelegatingSpecimenBuilderTransformation();
            this.sut.Add(dummyItem);
            // Assert
            Assert.True(verified);
        }

        [Fact]
        public void ClearNonEmptyCollectionRaisesGraphChanged()
        {
            // Arrange
            this.sut.Add(new DelegatingSpecimenBuilderTransformation());
            var verified = false;
            this.sut.GraphChanged += (s, e) => verified = s != null && e != null && e.Graph == this.sut.Graph;
            // Act
            this.sut.Clear();
            // Assert
            Assert.True(verified);
        }

        [Fact]
        public void ClearEmptyCollectionDoesNotRaiseGraphChanged()
        {
            // Arrange
            this.sut.Clear();
            var invoked = false;
            this.sut.GraphChanged += (s, e) => invoked = true;
            // Act
            this.sut.Clear();
            // Assert
            Assert.False(invoked);
        }

        [Fact]
        public void RemoveContainedItemRaisesGraphChanged()
        {
            // Arrange
            var item = new DelegatingSpecimenBuilderTransformation();
            this.sut.Add(item);

            var verified = false;
            this.sut.GraphChanged += (s, e) => verified = s != null && e != null && e.Graph == this.sut.Graph;
            // Act
            this.sut.Remove(item);
            // Assert
            Assert.True(verified);
        }

        [Fact]
        public void RemoveUncontainedItemDoesNotRaiseGraphChanged()
        {
            // Arrange
            var item = new DelegatingSpecimenBuilderTransformation();
            var invoked = false;
            this.sut.GraphChanged += (s, e) => invoked = true;
            // Act
            this.sut.Remove(item);
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
            this.sut.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode("A", b) });
            this.sut.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode("B", b) });
            this.sut.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode("C", b) });
            // Act
            var item = new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode(index, b) };
            this.sut.Insert(index, item);
            // Assert
            var expected = this.sut.Aggregate(
                this.graph,
                (b, t) => (ISpecimenBuilderNode)t.Transform(b));

            Assert.True(expected.GraphEquals(this.sut.Graph,
                new TaggedNodeComparer(new TrueComparer<ISpecimenBuilder>())));
        }

        [Fact]
        public void ClearCorrectlyChangesGraph()
        {
            // Arrange
            this.sut.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode("A", b) });
            this.sut.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode("B", b) });
            this.sut.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode("C", b) });
            // Act
            this.sut.Clear();
            // Assert
            Assert.True(this.graph.GraphEquals(this.sut.Graph,
                new TrueComparer<ISpecimenBuilder>()));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void RemoveAtCorrectlyChangesGraph(int index)
        {
            // Arrange
            this.sut.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode("A", b) });
            this.sut.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode("B", b) });
            this.sut.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode("C", b) });
            // Act
            this.sut.RemoveAt(index);
            // Assert
            var expected = this.sut.Aggregate(
                this.graph,
                (b, t) => (ISpecimenBuilderNode)t.Transform(b));

            Assert.True(expected.GraphEquals(this.sut.Graph,
                new TaggedNodeComparer(new TrueComparer<ISpecimenBuilder>())));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void SetItemCorrectlyChangesGraph(int index)
        {
            // Arrange
            this.sut.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode("A", b) });
            this.sut.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode("B", b) });
            this.sut.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode("C", b) });
            // Act
            var item = new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode(index, b) };
            this.sut[index] = item;
            // Assert
            var expected = this.sut.Aggregate(
                this.graph,
                (b, t) => (ISpecimenBuilderNode)t.Transform(b));

            Assert.True(expected.GraphEquals(this.sut.Graph,
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
                this.graph, n => n is MarkerNode, x, y, z);
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
}
