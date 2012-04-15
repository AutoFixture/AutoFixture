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
        public void SutIsNode()
        {
            // Fixture setup
            var dummyType = typeof(object);
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Exercise system
            var sut = new TypedNode(dummyType, dummyBuilder);
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilderNode>(sut);
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

            Assert.True(expected.GraphEquals(((ISpecimenBuilderNode)sut.Single()), new TypedNodeComparer()));
            Assert.True(expected.GraphEquals(((System.Collections.IEnumerable)sut).Cast<ISpecimenBuilderNode>().Single(), new TypedNodeComparer()));
            // Teardown
        }

        private class TypedNodeComparer : IEqualityComparer<ISpecimenBuilder>
        {
            private readonly IEqualityComparer<IRequestSpecification> specificationComparer;

            public TypedNodeComparer()
            {
                this.specificationComparer = new SpecificationComparer();
            }

            public bool Equals(ISpecimenBuilder x, ISpecimenBuilder y)
            {
                var fx = x as FilteringSpecimenBuilder;
                var fy = y as FilteringSpecimenBuilder;
                if (fx != null &&
                    fy != null &&
                    this.specificationComparer.Equals(fx.Specification, fy.Specification))
                    return true;

                if (x is CompositeSpecimenBuilder && y is CompositeSpecimenBuilder)
                    return true;

                var gx = x as NoSpecimenOutputGuard;
                var gy = y as NoSpecimenOutputGuard;
                if (gx != null &&
                    gy != null &&
                    this.specificationComparer.Equals(gx.Specification, gy.Specification))
                    return true;

                var sirx = x as SeedIgnoringRelay;
                var siry = y as SeedIgnoringRelay;
                if (sirx != null && siry != null)
                {
                    return true;
                }

                return EqualityComparer<ISpecimenBuilder>.Default.Equals(x, y);
            }

            public int GetHashCode(ISpecimenBuilder obj)
            {
                return EqualityComparer<ISpecimenBuilder>.Default.GetHashCode(obj);
            }

            private class SpecificationComparer : IEqualityComparer<IRequestSpecification>
            {
                public bool Equals(IRequestSpecification x, IRequestSpecification y)
                {
                    var invx = x as InverseRequestSpecification;
                    var invy = y as InverseRequestSpecification;
                    if (invx != null &&
                        invy != null &&
                        this.Equals(invx.Specification, invy.Specification))
                        return true;

                    var ox = x as OrRequestSpecification;
                    var oy = y as OrRequestSpecification;
                    if (ox != null &&
                        oy != null &&
                        ox.Specifications.SequenceEqual(oy.Specifications, this))
                        return true;

                    var sx = x as SeedRequestSpecification;
                    var sy = y as SeedRequestSpecification;
                    if (sx != null && sy != null)
                    {
                        if (sx.TargetType == sy.TargetType)
                            return true;
                    }

                    var ex = x as ExactTypeSpecification;
                    var ey = y as ExactTypeSpecification;
                    if (ex != null && ey != null)
                    {
                        if (ex.TargetType == ey.TargetType)
                            return true;
                    }

                    return EqualityComparer<IRequestSpecification>.Default.Equals(x, y);
                }

                public int GetHashCode(IRequestSpecification obj)
                {
                    return EqualityComparer<IRequestSpecification>.Default.GetHashCode(obj);
                }
            }
        }
    }
}
