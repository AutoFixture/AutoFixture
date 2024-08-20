using AutoFixture.Kernel;

namespace AutoFixture.Xunit3.Internal
{
    internal static class CustomizationExtensions
    {
        public static object Resolve(this IFixture source, object request)
            => new SpecimenContext(source).Resolve(request);
    }
}