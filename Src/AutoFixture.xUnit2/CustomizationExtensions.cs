using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;

namespace AutoFixture.Xunit2
{
    internal static class CustomizationExtensions
    {
        public static IEnumerable<ICustomization> GetCustomizations(this ParameterInfo source)
            => source
                .GetCustomAttributes<Attribute>(false)
                .OfType<IParameterCustomizationSource>()
                .OrderBy(x => x, new CustomizeAttributeComparer())
                .Select(x => x.GetCustomization(source));

        public static object Resolve(this IFixture source, ParameterInfo parameterInfo)
        {
            return new SpecimenContext(source).Resolve(parameterInfo);
        }
    }
}