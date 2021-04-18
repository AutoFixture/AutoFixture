using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Kernel;
using NUnit.Framework.Interfaces;

namespace AutoFixture.NUnit3
{
    internal static class CustomizationExtensions
    {
        private static ICustomization Aggregate(this IEnumerable<ICustomization> customizations)
        {
            return new CompositeCustomization(customizations);
        }

        public static void Customize(this IEnumerable<IParameterInfo> source, IFixture fixture)
        {
            source
                .Select(
                    x => x.GetCustomAttributes<Attribute>(false)
                        .OfType<IParameterCustomizationSource>()
                        .OrderBy(y => y, new CustomizeAttributeComparer())
                        .Select(y => y.GetCustomization(x.ParameterInfo))
                        .Aggregate())
                .Aggregate()
                .Customize(fixture);
        }

        public static object Resolve(this IFixture source, IParameterInfo parameterInfo)
        {
            return new SpecimenContext(source).Resolve(parameterInfo.ParameterInfo);
        }
    }
}