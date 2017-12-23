using System;
using AutoFixture.Dsl;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class SpecimenBuilderNodeFactoryTests
    {
        [Fact]
        public void CreateComposerReturnsCorrectResult()
        {
            // Arrange
            // Act
            NodeComposer<int> actual =
                SpecimenBuilderNodeFactory.CreateComposer<int>();
            // Assert
            var expected = new NodeComposer<int>(
                SpecimenBuilderNodeFactory.CreateTypedNode(
                    typeof(int),
                    new MethodInvoker(
                        new ModestConstructorQuery())));
            Assert.True(expected.GraphEquals(actual, new NodeComparer()));
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        [InlineData(typeof(Guid))]
        [InlineData(typeof(Version))]
        public void CreateTypedNodeReturnsCorrectResult(Type targetType)
        {
            // Arrange
            var factory = new DelegatingSpecimenBuilder();
            // Act
            FilteringSpecimenBuilder actual =
                SpecimenBuilderNodeFactory.CreateTypedNode(
                    targetType,
                    factory);
            // Assert
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
        }
    }
}
