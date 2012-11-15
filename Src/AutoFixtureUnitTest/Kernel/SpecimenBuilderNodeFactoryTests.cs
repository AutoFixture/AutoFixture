using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixture.Dsl;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class SpecimenBuilderNodeFactoryTests
    {
        [Fact]
        public void CreateComposerReturnsCorrectResult()
        {
            // Fixture setup
            // Exercise system
            NodeComposer<int> actual = 
                SpecimenBuilderNodeFactory.CreateComposer<int>();
            // Verify outcome
            var expected = new NodeComposer<int>(
                new TypedNode(
                    typeof(int),
                    new MethodInvoker(
                        new ModestConstructorQuery())));
            Assert.True(expected.GraphEquals(actual, new NodeComparer()));
            // Teardown
        }
    }
}
