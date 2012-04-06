using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixture;
using Ploeh.AutoFixtureUnitTest.Kernel;

namespace Ploeh.AutoFixtureUnitTest
{
    public class ResidueCollectorNodeTest
    {
        [Fact]
        public void SutIsCompositeSpecimenBuilder()
        {
            var sut = new ResidueCollectorNode();
            Assert.IsAssignableFrom<CompositeSpecimenBuilder>(sut);
        }

        [Fact]
        public void SutYieldsInjectedArray()
        {
            var expected = new[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            };
            var sut = new ResidueCollectorNode(expected);

            Assert.True(expected.SequenceEqual(sut));
        }

        [Fact]
        public void SutYieldsInjectedSequence()
        {
            var expected = new ISpecimenBuilder[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            }.AsEnumerable();
            var sut = new ResidueCollectorNode(expected);

            Assert.True(expected.SequenceEqual(sut));
        }

        [Fact]
        public void ComposeReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new ResidueCollectorNode();
            // Exercise system
            var expected = new[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            };
            var actual = sut.Compose(expected);
            // Verify outcome
            var rcn = Assert.IsAssignableFrom<ResidueCollectorNode>(actual);
            Assert.True(expected.SequenceEqual(rcn));
            // Teardown
        }
    }
}
