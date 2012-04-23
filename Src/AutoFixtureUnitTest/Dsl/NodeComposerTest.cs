using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Dsl;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;

namespace Ploeh.AutoFixtureUnitTest.Dsl
{
    public class NodeComposerTest
    {
        [Fact]
        public void SutIsComposer()
        {
            // Fixture setup
            // Exercise system
            var sut = new NodeComposer<object>();
            // Verify outcome
            Assert.IsAssignableFrom<ICustomizationComposer<object>>(sut);
            // Teardown
        }

        [Fact]
        public void SutIsFilter()
        {
            // Fixture setup
            // Exercise system
            var sut = new NodeComposer<string>();
            // Verify outcome
            Assert.IsAssignableFrom<FilteringSpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void SutIsCorrectInitialGraph()
        {
            // Fixture setup
            var sut = new NodeComposer<int>();
            // Exercise system
            // Verify outcome
            var factory = new MethodInvoker(new ModestConstructorQuery());
            var expected = new TypedNode(typeof(int), factory);
            Assert.True(expected.GraphEquals(sut, new NodeComparer()));
            // Teardown
        }
    }
}
