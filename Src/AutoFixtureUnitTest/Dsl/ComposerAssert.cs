using System;
using System.Linq;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Dsl
{
    internal static class ComposerAssert
    {
        internal static FilteringSpecimenBuilder IsFilter(this ISpecimenBuilder builder)
        {
            var filter = Assert.IsAssignableFrom<FilteringSpecimenBuilder>(builder);
            return filter;
        }

        internal static FilteringSpecimenBuilder ShouldContain(this FilteringSpecimenBuilder filter, Func<ISpecimenBuilder, bool> predicate)
        {
            var composite = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(filter.Builder);
            Assert.True(composite.Any(predicate));
            return filter;
        }

        internal static FilteringSpecimenBuilder ShouldSpecify<T>(this FilteringSpecimenBuilder filter)
        {
            var orSpec = Assert.IsAssignableFrom<OrRequestSpecification>(filter.Specification);

            var seedSpec = Assert.IsAssignableFrom<SeedRequestSpecification>(orSpec.Specifications.First());
            Assert.Equal(typeof(T), seedSpec.TargetType);

            var typeSpec = Assert.IsAssignableFrom<ExactTypeSpecification>(orSpec.Specifications.Skip(1).First());
            Assert.Equal(typeof(T), typeSpec.TargetType);

            return filter;
        }
    }
}
