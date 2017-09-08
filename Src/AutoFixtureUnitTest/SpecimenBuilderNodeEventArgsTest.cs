using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest
{
    public class SpecimenBuilderNodeEventArgsTest
    {
        [Fact]
        public void SutIsEventArgs()
        {
            // Fixture setup
            var dummyNode = new CompositeSpecimenBuilder();
            // Exercise system
            var sut = new SpecimenBuilderNodeEventArgs(dummyNode);
            // Verify outcome
            Assert.IsAssignableFrom<EventArgs>(sut);
            // Teardown
        }

        [Fact]
        public void GraphIsCorrect()
        {
            // Fixture setup
            var expected = new CompositeSpecimenBuilder();
            var sut = new SpecimenBuilderNodeEventArgs(expected);
            // Exercise system
            ISpecimenBuilderNode actual = sut.Graph;
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void ConstructWithNullGraphThrows()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new SpecimenBuilderNodeEventArgs(null));
        }
    }
}
