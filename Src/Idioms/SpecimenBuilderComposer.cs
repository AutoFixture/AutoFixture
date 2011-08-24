using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Idioms
{
    internal static class SpecimenBuilderComposer
    {
        internal static object CreateAnonymous(this ISpecimenBuilderComposer composer, object request)
        {
            return new SpecimenContext(composer.Compose()).Resolve(request);
        }
    }
}
