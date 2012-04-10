using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class SpecimenBuilderNodeTest
    {
        [Fact]
        public void GraphEqualsNullFirstThrows()
        {
            var dummyNode = new CompositeSpecimenBuilder();
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenBuilderNode.GraphEquals(null, dummyNode));
        }

        [Fact]
        public void GraphEqualsNullSecondThrows()
        {
            var dummyNode = new CompositeSpecimenBuilder();
            Assert.Throws<ArgumentNullException>(() =>
                dummyNode.GraphEquals(null));
        }

        [Fact]
        public void GraphEqualsNullFirstWithEqualityComparerThrows()
        {
            var dummyNode = new CompositeSpecimenBuilder();
            var dummyEqualityComparer = new DelegatingEqualityComparer<ISpecimenBuilder>();
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenBuilderNode.GraphEquals(null, dummyNode, dummyEqualityComparer));
        }

        [Fact]
        public void GraphEqualsNullSecondWithEqualityComparerThrows()
        {
            var dummyNode = new CompositeSpecimenBuilder();
            var dummyEqualityComparer = new DelegatingEqualityComparer<ISpecimenBuilder>();
            Assert.Throws<ArgumentNullException>(() =>
                dummyNode.GraphEquals(null, dummyEqualityComparer));
        }

        [Fact]
        public void GraphEqualsWithNullEqualityComparerThrows()
        {
            var dummyNode1 = new CompositeSpecimenBuilder();
            var dummyNode2 = new CompositeSpecimenBuilder();
            Assert.Throws<ArgumentNullException>(() =>
                dummyNode1.GraphEquals(dummyNode2, null));
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void EmptyNodesAreEqualAccordingToComparer(bool expected)
        {
            // Fixture setup
            var node1 = new CompositeSpecimenBuilder();
            var node2 = new CompositeSpecimenBuilder();
            var comparer = new DelegatingEqualityComparer<ISpecimenBuilder>
            {
                OnEquals = (x, y) => x == node1 && y == node2 && expected
            };
            // Exercise system
            var actual = node1.GraphEquals(node2, comparer);
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Theory, ClassData(typeof(IdenticallyShapedGraphs))]
        public void NodesWithIdenticalShapesAreEqualWhenComparerIsAlwaysTrue(
            ISpecimenBuilderNode first,
            ISpecimenBuilderNode second)
        {
            // Fixture setup
            var trueComparer = new DelegatingEqualityComparer<ISpecimenBuilder>
            {
                OnEquals = (x, y) => true
            };
            // Exercise system
            var actual = first.GraphEquals(second, trueComparer);
            // Verify outcome
            Assert.True(actual);
            // Teardown
        }

        [Theory, ClassData(typeof(DifferentlyShapedGraphs))]
        public void NodesWithDifferentShapesAreNotEqualEvenWhenComparerIsAlwaysTrue(
            ISpecimenBuilderNode first,
            ISpecimenBuilderNode second)
        {
            // Fixture setup
            var trueComparer = new DelegatingEqualityComparer<ISpecimenBuilder>
            {
                OnEquals = (x, y) => true
            };
            // Exercise system
            var actual = first.GraphEquals(second, trueComparer);
            // Verify outcome
            Assert.False(actual);
            // Teardown
        }

        private class IdenticallyShapedGraphs : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] 
                {
                    new CompositeSpecimenBuilder(),
                    new CompositeSpecimenBuilder()
                };
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        private class DifferentlyShapedGraphs : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new CompositeSpecimenBuilder(),
                    new CompositeSpecimenBuilder(
                        new DelegatingSpecimenBuilder())
                };
                yield return new object[]
                {
                    new CompositeSpecimenBuilder(
                        new DelegatingSpecimenBuilder()),
                    new CompositeSpecimenBuilder(new []{
                        new CompositeSpecimenBuilder(
                            new DelegatingSpecimenBuilder())})                };
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }
    }
}
