using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class GraphEqualsTest
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
            var trueComparer = new TrueComparer<ISpecimenBuilder>();
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
            var trueComparer = new TrueComparer<ISpecimenBuilder>();
            // Exercise system
            var actual = first.GraphEquals(second, trueComparer);
            // Verify outcome
            Assert.False(actual);
            // Teardown
        }

        [Theory, ClassData(typeof(SameGraphs))]
        public void IdenticalNodesAreEqual(
            ISpecimenBuilderNode first,
            ISpecimenBuilderNode second)
        {
            // Fixture setup
            // Exercise system
            var actual = first.GraphEquals(second);
            // Verify outcome
            Assert.True(actual);
            // Teardown
        }

        [Theory]
        [ClassData(typeof(DifferentlyShapedGraphs))]
        [ClassData(typeof(IdenticallyShapedGraphs))]
        public void DifferentNodesAreNotEqualWhenComparerIsOmitted(
            ISpecimenBuilderNode first,
            ISpecimenBuilderNode second)
        {
            // Fixture setup
            // Exercise system
            var actual = first.GraphEquals(second);
            // Verify outcome
            Assert.False(actual);
            // Teardown
        }

        [Theory]
        [ClassData(typeof(SimilarTaggedGraphs))]
        public void SimilarNodesAreEqualAccordingToTags(
            ISpecimenBuilderNode first,
            ISpecimenBuilderNode second)
        {
            // Fixture setup
            // Exercise system
            var actual = first.GraphEquals(second, new TaggedNodeComparer());
            // Verify outcome
            Assert.True(actual);
            // Teardown
        }

        [Theory]
        [ClassData(typeof(DifferentTaggedGraphs))]
        public void DifferentNodesAreNotEqualAccordingToTags(
            ISpecimenBuilderNode first,
            ISpecimenBuilderNode second)
        {
            // Fixture setup
            // Exercise system
            var actual = first.GraphEquals(second, new TaggedNodeComparer());
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
                yield return new object[]
                {
                    new CompositeSpecimenBuilder(
                        new DelegatingSpecimenBuilder()),
                    new CompositeSpecimenBuilder(
                        new[] { new CompositeSpecimenBuilder() })
                };
                yield return new object[]
                {
                    new CompositeSpecimenBuilder(
                        new DelegatingSpecimenBuilder(),
                        new DelegatingSpecimenBuilder()),
                    new CompositeSpecimenBuilder(
                        new DelegatingSpecimenBuilder(),
                        new CompositeSpecimenBuilder())
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
                            new DelegatingSpecimenBuilder())})
                };
                yield return new object[]
                {
                    new CompositeSpecimenBuilder(
                        new DelegatingSpecimenBuilder(),
                        new CompositeSpecimenBuilder(
                            new DelegatingSpecimenBuilder())),
                    new CompositeSpecimenBuilder(
                        new DelegatingSpecimenBuilder(),
                        new CompositeSpecimenBuilder())
                };
                yield return new object[]
                {
                    new CompositeSpecimenBuilder(
                        new DelegatingSpecimenBuilder(),
                        new DelegatingSpecimenBuilder(),
                        new DelegatingSpecimenBuilder()),
                    new CompositeSpecimenBuilder(
                        new DelegatingSpecimenBuilder(),
                        new DelegatingSpecimenBuilder())
                };
                yield return new object[]
                {
                    new CompositeSpecimenBuilder(
                        new DelegatingSpecimenBuilder(),
                        new DelegatingSpecimenBuilder()),
                    new CompositeSpecimenBuilder(
                        new DelegatingSpecimenBuilder(),
                        new DelegatingSpecimenBuilder(),
                        new DelegatingSpecimenBuilder())
                };
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        private class SameGraphs : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                var g1 = new CompositeSpecimenBuilder();
                yield return new object[] { g1, g1 };

                var g2 = new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder());
                yield return new object[] { g2, g2 };
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        private class SimilarTaggedGraphs : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                var leaf = new DelegatingSpecimenBuilder();

                yield return new object[]
                {
                    new TaggedNode(1),
                    new TaggedNode(1)
                };
                yield return new object[]
                {
                    new TaggedNode(1,
                        new TaggedNode(2),
                        new TaggedNode(3)),
                    new TaggedNode(1,
                        new TaggedNode(2),
                        new TaggedNode(3))
                };
                yield return new object[]
                {
                    new TaggedNode(1, leaf),
                    new TaggedNode(1, leaf)
                };
                yield return new object[]
                {
                    new TaggedNode(1,
                        new TaggedNode(2,
                            leaf,
                            leaf,
                            leaf),
                        new TaggedNode(3,
                            leaf,
                            leaf,
                            leaf)),
                    new TaggedNode(1,
                        new TaggedNode(2,
                            leaf,
                            leaf,
                            leaf),
                        new TaggedNode(3,
                            leaf,
                            leaf,
                            leaf))
                };
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        private class DifferentTaggedGraphs : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new TaggedNode(1),
                    new TaggedNode("A")
                };
                yield return new object[]
                {
                    new TaggedNode(1,
                        new TaggedNode(2),
                        new TaggedNode(3)),
                    new TaggedNode(1,
                        new TaggedNode(2),
                        new TaggedNode("A"))
                };
                yield return new object[]
                {
                    new TaggedNode(1,
                        new DelegatingSpecimenBuilder()),
                    new TaggedNode(1,
                        new DelegatingSpecimenBuilder())
                };
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }
    }
}
