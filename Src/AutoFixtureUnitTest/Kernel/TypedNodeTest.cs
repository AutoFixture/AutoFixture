using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class TypedNodeTest
    {
        [Fact]
        public void SutIsFilter()
        {
            // Fixture setup
            var dummyType = typeof(object);
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Exercise system
            var sut = new TypedNode(dummyType, dummyBuilder);
            // Verify outcome
            Assert.IsAssignableFrom<FilteringSpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void ConstructWithNullTypeThrows()
        {
            var dummyBuilder = new DelegatingSpecimenBuilder();
            Assert.Throws<ArgumentNullException>(() =>
                new TypedNode(null, dummyBuilder));
        }

        [Fact]
        public void ConstructWithNullFactoryThrows()
        {
            var dummyType = typeof(object);
            Assert.Throws<ArgumentNullException>(() =>
                new TypedNode(dummyType, null));
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        [InlineData(typeof(Guid))]
        [InlineData(typeof(Version))]
        public void TargetTypeIsCorrect(Type expected)
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new TypedNode(expected, dummyBuilder);
            // Exercise system
            Type actual = sut.TargetType;
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void FactoryIsCorrect()
        {
            // Fixture setup
            var dummyType = typeof(object);
            var expected = new DelegatingSpecimenBuilder();
            var sut = new TypedNode(dummyType, expected);
            // Exercise system
            ISpecimenBuilder actual = sut.Factory;
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        [InlineData(typeof(Guid))]
        [InlineData(typeof(Version))]
        public void SutYieldsCorrectDescendants(Type targetType)
        {
            // Fixture setup
            var factory = new DelegatingSpecimenBuilder();
            var sut = new TypedNode(targetType, factory);
            // Exercise system
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

            Assert.True(expected.GraphEquals(sut, new NodeComparer()));
            // Teardown
        }
    }
}
