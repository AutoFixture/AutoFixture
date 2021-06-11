using System.Reflection;
using AutoFixture.Kernel;

namespace AutoFixture.Xunit2.Internal
{
    internal static class CustomizationExtensions
    {
        public static object Resolve(this IFixture source, ParameterInfo parameterInfo)
            => new SpecimenContext(source).Resolve(parameterInfo);
    }
}