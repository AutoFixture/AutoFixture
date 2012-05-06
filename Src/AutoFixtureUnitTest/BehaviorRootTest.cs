using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;

namespace Ploeh.AutoFixtureUnitTest
{
    public class BehaviorRootTest
    {
        [Fact]
        public void SutIsCompositeSpecimenBuilder()
        {
            var sut = new BehaviorRoot();
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
            var sut = new BehaviorRoot(expected);

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
            var sut = new BehaviorRoot(expected);

            Assert.True(expected.SequenceEqual(sut));
        }

        [Fact]
        public void ComposeReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new BehaviorRoot();
            // Exercise system
            var expected = new[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            };
            var actual = sut.Compose(expected);
            // Verify outcome
            var br = Assert.IsAssignableFrom<BehaviorRoot>(actual);
            Assert.True(expected.SequenceEqual(br));
            // Teardown
        }
    }
}
