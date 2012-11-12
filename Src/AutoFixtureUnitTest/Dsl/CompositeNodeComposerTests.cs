using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixture.Dsl;

namespace Ploeh.AutoFixtureUnitTest.Dsl
{
    public class CompositeNodeComposerTests
    {
        [Fact]
        public void SutIsComposer()
        {
            // Fixture setup
            var dummyNode = new CompositeSpecimenBuilder();
            // Exercise system
            var sut = new CompositeNodeComposer<object>(dummyNode);
            // Verify outcome
            Assert.IsAssignableFrom<ICustomizationComposer<object>>(sut);
            // Teardown
        }

        [Fact]
        public void SutIsSpecimenBuilderNode()
        {
            // Fixture setup
            var dummyNode = new CompositeSpecimenBuilder();
            // Exercise system
            var sut = new CompositeNodeComposer<float>(dummyNode);
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilderNode>(sut);
            // Teardown
        }

        [Fact]
        public void ComposeReturnsCorrectResult()
        {
            // Fixture setup
            var dummyNode = new CompositeSpecimenBuilder();
            var sut = new CompositeNodeComposer<uint>(dummyNode);
            // Exercise system
            var expectedBuilders = new[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            };
            var actual = sut.Compose(expectedBuilders);
            // Verify outcome
            var c =
                Assert.IsAssignableFrom<CompositeNodeComposer<uint>>(actual);
            var composite = 
                Assert.IsAssignableFrom<CompositeSpecimenBuilder>(c.Node);
            Assert.True(expectedBuilders.SequenceEqual(composite));
            // Teardown
        }

        [Fact]
        public void NodeIsCorrect()
        {
            // Fixture setup
            var expected = new CompositeSpecimenBuilder();
            var sut = new CompositeNodeComposer<string>(expected);
            // Exercise system
            ISpecimenBuilderNode actual = sut.Node;
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void ConstructWithNullNodeThrows()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeNodeComposer<Guid>(null));
        }
    }
}
