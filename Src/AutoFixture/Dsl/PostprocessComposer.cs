using System.Linq;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Dsl
{
    public static class PostprocessComposer
    {
        public static IMatchComposer<T> Match<T>(
            this IPostprocessComposer<T> composer)
        {
            var graph = composer as ISpecimenBuilderNode;

            if (graph == null)
            {
                return new NullMatchComposer<T>();
            }

            var container = graph
                .SelectNodes(n => n is FilteringSpecimenBuilder)
                .Cast<FilteringSpecimenBuilder>()
                .First();
            return new MatchComposer<T>(container.Builder);
        }
    }
}
