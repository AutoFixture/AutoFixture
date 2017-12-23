using System;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
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
            // Arrange
            var node1 = new CompositeSpecimenBuilder();
            var node2 = new CompositeSpecimenBuilder();
            var comparer = new DelegatingEqualityComparer<ISpecimenBuilder>
            {
                OnEquals = (x, y) => x == node1 && y == node2 && expected
            };
            // Act
            var actual = node1.GraphEquals(node2, comparer);
            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(IdenticallyShapedGraphs))]
        public void NodesWithIdenticalShapesAreEqualWhenComparerIsAlwaysTrue(
            ISpecimenBuilderNode first,
            ISpecimenBuilderNode second)
        {
            // Arrange
            var trueComparer = new TrueComparer<ISpecimenBuilder>();
            // Act
            var actual = first.GraphEquals(second, trueComparer);
            // Assert
            Assert.True(actual);
        }

        [Theory]
        [MemberData(nameof(DifferentlyShapedGraphs))]
        public void NodesWithDifferentShapesAreNotEqualEvenWhenComparerIsAlwaysTrue(
            ISpecimenBuilderNode first,
            ISpecimenBuilderNode second)
        {
            // Arrange
            var trueComparer = new TrueComparer<ISpecimenBuilder>();
            // Act
            var actual = first.GraphEquals(second, trueComparer);
            // Assert
            Assert.False(actual);
        }

        [Theory]
        [MemberData(nameof(SameGraphs))]
        public void IdenticalNodesAreEqual(
            ISpecimenBuilderNode first,
            ISpecimenBuilderNode second)
        {
            // Arrange
            // Act
            var actual = first.GraphEquals(second);
            // Assert
            Assert.True(actual);
        }

        [Theory]
        [MemberData(nameof(DifferentlyShapedGraphs))]
        [MemberData(nameof(IdenticallyShapedGraphs))]
        public void DifferentNodesAreNotEqualWhenComparerIsOmitted(
            ISpecimenBuilderNode first,
            ISpecimenBuilderNode second)
        {
            // Arrange
            // Act
            var actual = first.GraphEquals(second);
            // Assert
            Assert.False(actual);
        }

        [Theory]
        [MemberData(nameof(SimilarTaggedGraphs))]
        public void SimilarNodesAreEqualAccordingToTags(
            ISpecimenBuilderNode first,
            ISpecimenBuilderNode second)
        {
            // Arrange
            // Act
            var actual = first.GraphEquals(second, new TaggedNodeComparer());
            // Assert
            Assert.True(actual);
        }

        [Theory]
        [MemberData(nameof(DifferentTaggedGraphs))]
        public void DifferentNodesAreNotEqualAccordingToTags(
            ISpecimenBuilderNode first,
            ISpecimenBuilderNode second)
        {
            // Arrange
            // Act
            var actual = first.GraphEquals(second, new TaggedNodeComparer());
            // Assert
            Assert.False(actual);
        }

        public static TheoryData<ISpecimenBuilderNode, ISpecimenBuilderNode> IdenticallyShapedGraphs =>
            new TheoryData<ISpecimenBuilderNode, ISpecimenBuilderNode>
            {
                {
                    new CompositeSpecimenBuilder(),
                    new CompositeSpecimenBuilder()
                },
                {
                    new CompositeSpecimenBuilder(
                        new DelegatingSpecimenBuilder()),
                    new CompositeSpecimenBuilder(
                        new[] {new CompositeSpecimenBuilder()})
                },
                {
                    new CompositeSpecimenBuilder(
                        new DelegatingSpecimenBuilder(),
                        new DelegatingSpecimenBuilder()),
                    new CompositeSpecimenBuilder(
                        new DelegatingSpecimenBuilder(),
                        new CompositeSpecimenBuilder())
                }
            };

        public static TheoryData<ISpecimenBuilderNode, ISpecimenBuilderNode> DifferentlyShapedGraphs =>
            new TheoryData<ISpecimenBuilderNode, ISpecimenBuilderNode>

            {
                {
                    new CompositeSpecimenBuilder(),
                    new CompositeSpecimenBuilder(
                        new DelegatingSpecimenBuilder())
                },
                {
                    new CompositeSpecimenBuilder(
                        new DelegatingSpecimenBuilder()),
                    new CompositeSpecimenBuilder(new[]
                    {
                        new CompositeSpecimenBuilder(
                            new DelegatingSpecimenBuilder())
                    })
                },
                {
                    new CompositeSpecimenBuilder(
                        new DelegatingSpecimenBuilder(),
                        new CompositeSpecimenBuilder(
                            new DelegatingSpecimenBuilder())),
                    new CompositeSpecimenBuilder(
                        new DelegatingSpecimenBuilder(),
                        new CompositeSpecimenBuilder())
                },
                {
                    new CompositeSpecimenBuilder(
                        new DelegatingSpecimenBuilder(),
                        new DelegatingSpecimenBuilder(),
                        new DelegatingSpecimenBuilder()),
                    new CompositeSpecimenBuilder(
                        new DelegatingSpecimenBuilder(),
                        new DelegatingSpecimenBuilder())
                },
                {
                    new CompositeSpecimenBuilder(
                        new DelegatingSpecimenBuilder(),
                        new DelegatingSpecimenBuilder()),
                    new CompositeSpecimenBuilder(
                        new DelegatingSpecimenBuilder(),
                        new DelegatingSpecimenBuilder(),
                        new DelegatingSpecimenBuilder())
                }
            };

        public static TheoryData<ISpecimenBuilderNode, ISpecimenBuilderNode> SameGraphs
        {
            get
            {
                var g1 = new CompositeSpecimenBuilder();
                var g2 = new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder());

                return new TheoryData<ISpecimenBuilderNode, ISpecimenBuilderNode>
                {
                    { g1, g1 },
                    { g2, g2 }
                };
            }
        }

        public static TheoryData<ISpecimenBuilderNode, ISpecimenBuilderNode> SimilarTaggedGraphs
        {
            get
            {
                var leaf = new DelegatingSpecimenBuilder();

                return new TheoryData<ISpecimenBuilderNode, ISpecimenBuilderNode>
                {
                    {
                        new TaggedNode(1),
                        new TaggedNode(1)
                    },
                    {
                        new TaggedNode(1,
                            new TaggedNode(2),
                            new TaggedNode(3)),
                        new TaggedNode(1,
                            new TaggedNode(2),
                            new TaggedNode(3))
                    },
                    {
                        new TaggedNode(1, leaf),
                        new TaggedNode(1, leaf)
                    },
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
                    }
                };
            }
        }

        public static TheoryData<ISpecimenBuilderNode, ISpecimenBuilderNode> DifferentTaggedGraphs =>
            new TheoryData<ISpecimenBuilderNode, ISpecimenBuilderNode>
            {
                {
                    new TaggedNode(1),
                    new TaggedNode("A")
                },
                {
                    new TaggedNode(1,
                        new TaggedNode(2),
                        new TaggedNode(3)),
                    new TaggedNode(1,
                        new TaggedNode(2),
                        new TaggedNode("A"))
                },
                {
                    new TaggedNode(1,
                        new DelegatingSpecimenBuilder()),
                    new TaggedNode(1,
                        new DelegatingSpecimenBuilder())
                }
            };
    }
}
