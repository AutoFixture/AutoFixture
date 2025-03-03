using AutoFixture.Kernel;

namespace AutoFixture.TUnit.Internal
{
    internal static class CustomizationExtensions
    {
        public static object Resolve(this IFixture source, object request)
            => new SpecimenContext(source).Resolve(request);
    }
}