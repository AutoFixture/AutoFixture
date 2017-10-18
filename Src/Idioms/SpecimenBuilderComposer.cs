using AutoFixture.Kernel;

namespace AutoFixture.Idioms
{
    internal static class SpecimenBuilderComposer
    {
        internal static object CreateAnonymous(this ISpecimenBuilder builder, object request)
        {
            return new SpecimenContext(builder).Resolve(request);
        }
    }
}
