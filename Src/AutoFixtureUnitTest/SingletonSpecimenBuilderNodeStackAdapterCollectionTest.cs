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
            // Fixture setup
            // Exercise system
            ISpecimenBuilderNode actual = this.sut.Graph;
            // Verify outcome
            Assert.Equal(this.graph, actual);
            // Teardown
        }

        [Fact]
        public void InsertRaisesGraphChanged()
        {
            // Fixture setup
            var verified = false;
            this.sut.GraphChanged += (s, e) => verified = s != null && e != null && e.Graph == this.sut.Graph;
            // Exercise system
            var dummyIndex = 0;
            var dummyItem = new DelegatingSpecimenBuilderTransformation();
            this.sut.Insert(dummyIndex, dummyItem);
            // Verify outcome
            Assert.True(verified);
            // Teardown
        }

        [Fact]
        public void RemoveAtRaisesGraphChanged()
        {
            // Fixture setup
            this.sut.Add(new DelegatingSpecimenBuilderTransformation());
            var verified = false;
            this.sut.GraphChanged += (s, e) => verified = s != null && e != null && e.Graph == this.sut.Graph;
            // Exercise system
            var dummyIndex = 0;
            this.sut.RemoveAt(dummyIndex);
            // Verify outcome
            Assert.True(verified);
            // Teardown
        }

        [Fact]
        public void SetItemRaisesGraphChanged()
        {
            // Fixture setup
            this.sut.Add(new DelegatingSpecimenBuilderTransformation());
            var verified = false;
            this.sut.GraphChanged += (s, e) => verified = s != null && e != null && e.Graph == this.sut.Graph;
            // Exercise system
            var dummyIndex = 0;
            var dummyItem = new DelegatingSpecimenBuilderTransformation();
            this.sut[dummyIndex] = dummyItem;
            // Verify outcome
            Assert.True(verified);
            // Teardown
        }

        [Fact]
        public void AddRaisesGraphChanged()
        {
            // Fixture setup
            var verified = false;
            this.sut.GraphChanged += (s, e) => verified = s != null && e != null && e.Graph == this.sut.Graph;
            // Exercise system
            var dummyItem = new DelegatingSpecimenBuilderTransformation();
            this.sut.Add(dummyItem);
            // Verify outcome
            Assert.True(verified);
            // Teardown
        }

        [Fact]
        public void ClearNonEmptyCollectionRaisesGraphChanged()
        {
            // Fixture setup
            this.sut.Add(new DelegatingSpecimenBuilderTransformation());
            var verified = false;
            this.sut.GraphChanged += (s, e) => verified = s != null && e != null && e.Graph == this.sut.Graph;
            // Exercise system
            this.sut.Clear();
            // Verify outcome
            Assert.True(verified);
            // Teardown
        }

        [Fact]
        public void ClearEmptyCollectionDoesNotRaiseGraphChanged()
        {
            // Fixture setup
            this.sut.Clear();
            var invoked = false;
            this.sut.GraphChanged += (s, e) => invoked = true;
            // Exercise system
            this.sut.Clear();
            // Verify outcome
            Assert.False(invoked);
            // Teardown
        }

        [Fact]
        public void RemoveContainedItemRaisesGraphChanged()
        {
            // Fixture setup
            var item = new DelegatingSpecimenBuilderTransformation();
            this.sut.Add(item);

            var verified = false;
            this.sut.GraphChanged += (s, e) => verified = s != null && e != null && e.Graph == this.sut.Graph;
            // Exercise system
            this.sut.Remove(item);
            // Verify outcome
            Assert.True(verified);
            // Teardown
        }

        [Fact]
        public void RemoveUncontainedItemDoesNotRaiseGraphChanged()
        {
            // Fixture setup
            var item = new DelegatingSpecimenBuilderTransformation();
            var invoked = false;
            this.sut.GraphChanged += (s, e) => invoked = true;
            // Exercise system
            this.sut.Remove(item);
            // Verify outcome
            Assert.False(invoked);
            // Teardown
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void InsertItemCorrectlyChangesGraph(int index)
        {
            // Fixture setup
            this.sut.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode("A", b) });
            this.sut.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode("B", b) });
            this.sut.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode("C", b) });
            // Exercise system
            var item = new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode(index, b) };
            this.sut.Insert(index, item);
            // Verify outcome
            var expected = this.sut.Aggregate(
                this.graph,
                (b, t) => (ISpecimenBuilderNode)t.Transform(b));

            Assert.True(expected.GraphEquals(this.sut.Graph,
                new TaggedNodeComparer(new TrueComparer<ISpecimenBuilder>())));
            // Teardown
        }

        [Fact]
        public void ClearCorrectlyChangesGraph()
        {
            // Fixture setup
            this.sut.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode("A", b) });
            this.sut.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode("B", b) });
            this.sut.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode("C", b) });
            // Exercise system
            this.sut.Clear();
            // Verify outcome
            Assert.True(this.graph.GraphEquals(this.sut.Graph,
                new TrueComparer<ISpecimenBuilder>()));
            // Teardown
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void RemoveAtCorrectlyChangesGraph(int index)
        {
            // Fixture setup
            this.sut.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode("A", b) });
            this.sut.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode("B", b) });
            this.sut.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode("C", b) });
            // Exercise system
            this.sut.RemoveAt(index);
            // Verify outcome
            var expected = this.sut.Aggregate(
                this.graph,
                (b, t) => (ISpecimenBuilderNode)t.Transform(b));

            Assert.True(expected.GraphEquals(this.sut.Graph,
                new TaggedNodeComparer(new TrueComparer<ISpecimenBuilder>())));
            // Teardown
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void SetItemCorrectlyChangesGraph(int index)
        {
            // Fixture setup
            this.sut.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode("A", b) });
            this.sut.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode("B", b) });
            this.sut.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode("C", b) });
            // Exercise system
            var item = new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode(index, b) };
            this.sut[index] = item;
            // Verify outcome
            var expected = this.sut.Aggregate(
                this.graph,
                (b, t) => (ISpecimenBuilderNode)t.Transform(b));

            Assert.True(expected.GraphEquals(this.sut.Graph,
                new TaggedNodeComparer(new TrueComparer<ISpecimenBuilder>())));
            // Teardown
        }

        [Fact]
        public void SutContainsItemsFromConstructor()
        {
            // Fixture setup
            var x = new DelegatingSpecimenBuilderTransformation();
            var y = new DelegatingSpecimenBuilderTransformation();
            var z = new DelegatingSpecimenBuilderTransformation();
            // Exercise system
            var s = new SingletonSpecimenBuilderNodeStackAdapterCollection(
                this.graph, n => n is MarkerNode, x, y, z);
            // Verify outcome
            Assert.True(new[] { x, y, z }.SequenceEqual(s));
            // Teardown
        }

        [Fact]
        public void ConstructWithNullGraphThrows()
        {
            // Fixture setup
            Func<ISpecimenBuilderNode, bool> dummyPredicate = n => false;
            var dummyTransformations = new ISpecimenBuilderTransformation[0];
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new SingletonSpecimenBuilderNodeStackAdapterCollection(
                    null,
                    dummyPredicate,
                    dummyTransformations));
            // Teardown
        }

        [Fact]
        public void ConstructWithNullPredicateThrows()
        {
            // Fixture setup
            var dummyGraph = new CompositeSpecimenBuilder();
            var dummyTransformations = new ISpecimenBuilderTransformation[0];
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new SingletonSpecimenBuilderNodeStackAdapterCollection(
                    dummyGraph,
                    null,
                    dummyTransformations));
            // Teardown
        }

        [Fact]
        public void ConstructWithNullTransformationsThrows()
        {
            // Fixture setup
            var dummyGraph = new CompositeSpecimenBuilder();
            Func<ISpecimenBuilderNode, bool> dummyPredicate = n => false;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new SingletonSpecimenBuilderNodeStackAdapterCollection(
                    dummyGraph,
                    dummyPredicate,
                    null));
            // Teardown
        }
    }
}
