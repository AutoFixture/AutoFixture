using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixture;
using System.Collections.ObjectModel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest
{
    public class SingletonSpecimenBuilderNodeStackCollectionAdapterTest
    {
        private readonly ISpecimenBuilderNode graph;
        private readonly SingletonSpecimenBuilderNodeStackCollectionAdapter sut;

        public SingletonSpecimenBuilderNodeStackCollectionAdapterTest()
        {
            this.graph = new MarkerNode(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    new DelegatingSpecimenBuilder(),
                    new DelegatingSpecimenBuilder()),
                new CompositeSpecimenBuilder(new DelegatingSpecimenBuilder(),
                    new DelegatingSpecimenBuilder(),
                    new DelegatingSpecimenBuilder()),
                new CompositeSpecimenBuilder(new DelegatingSpecimenBuilder(),
                    new DelegatingSpecimenBuilder(),
                    new DelegatingSpecimenBuilder()));
            this.sut = new SingletonSpecimenBuilderNodeStackCollectionAdapter(this.graph);
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

        [Theory(Skip = "Waiting for a bit of test refactoring before this can be implement, because a bit more test setup is necessary.")]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void InsertItemCorrectlyChangesGraph(int index)
        {
            // Fixture setup
            this.sut.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new MarkerNode(b) { Tag = "A" } });
            this.sut.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new MarkerNode(b) { Tag = "B" } });
            this.sut.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new MarkerNode(b) { Tag = "C" } });            
            // Exercise system
            var item = new DelegatingSpecimenBuilderTransformation { OnTransform = b => new MarkerNode(b) { Tag = index } };
            this.sut.Insert(index, item);
            // Verify outcome
            var expected = this.sut.Aggregate(
                this.graph,
                (b, t) => (ISpecimenBuilderNode)t.Transform(b));

            Assert.True(expected.GraphEquals(this.sut.Graph, MarkerNode.Comparer));
            // Teardown
        }
    }
}
