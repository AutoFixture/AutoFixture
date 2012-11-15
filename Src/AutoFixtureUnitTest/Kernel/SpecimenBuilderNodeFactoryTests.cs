using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixture.Dsl;
using Xunit.Extensions;

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
                SpecimenBuilderNodeFactory.CreateTypedNode(
                    typeof(int),
                    new MethodInvoker(
                        new ModestConstructorQuery())));
            Assert.True(expected.GraphEquals(actual, new NodeComparer()));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        [InlineData(typeof(Guid))]
        [InlineData(typeof(Version))]
        public void CreateTypedNodeReturnsCorrectResult(Type targetType)
        {
            // Fixture setup
            var factory = new DelegatingSpecimenBuilder();
            // Exercise system
            FilteringSpecimenBuilder actual =
                SpecimenBuilderNodeFactory.CreateTypedNode(
                    targetType,
                    factory);
            // Verify outcome
            var expected = new FilteringSpecimenBuilder(
                new CompositeSpecimenBuilder(
                    new NoSpecimenOutputGuard(
                        factory,
                        new InverseRequestSpecification(
                            new SeedRequestSpecification(
                                targetType))),
                    new SeedIgnoringRelay()),
                new OrRequestSpecification(
                    new SeedRequestSpecification(targetType),
                    new ExactTypeSpecification(targetType)));

            Assert.True(expected.GraphEquals(actual, new NodeComparer()));
            // Teardown
        }
    }
}
