using System;
using System.Linq;
using System.Reflection;

namespace AutoFixture.Xunit3.Internal
{
    internal class TestParameter
    {
        private readonly Lazy<ICustomization> lazyCustomization;
        private readonly Lazy<FrozenAttribute> lazyFrozenAttribute;

        public TestParameter(ParameterInfo parameterInfo)
        {
            this.ParameterInfo = parameterInfo ?? throw new ArgumentNullException(nameof(parameterInfo));

            this.lazyCustomization = new Lazy<ICustomization>(
                () => GetCustomization(parameterInfo));
            this.lazyFrozenAttribute = new Lazy<FrozenAttribute>(
                () => parameterInfo.GetCustomAttributes()
                                   .OfType<FrozenAttribute>().FirstOrDefault());
        }

        public ParameterInfo ParameterInfo { get; }

        public ICustomization GetCustomization() => this.lazyCustomization.Value;

        public ICustomization GetCustomization(object value)
        {
            var frozenAttribute = this.lazyFrozenAttribute.Value;

            if (frozenAttribute is null)
            {
                return new NullCustomization();
            }

            return new FrozenValueCustomization(
                new ParameterFilter(this.ParameterInfo, frozenAttribute.By),
                value);
        }

        private static ICustomization GetCustomization(ParameterInfo parameter)
        {
            var customizations = parameter.GetCustomAttributes()
                                          .OfType<IParameterCustomizationSource>()
                                          .OrderBy(x => x, new CustomizeAttributeComparer())
                                          .Select(x => x.GetCustomization(parameter))
                                          .ToArray();

            return customizations switch
            {
                { Length: 0 } => new NullCustomization(),
                { Length: 1 } => customizations[0],
                _ => new CompositeCustomization(customizations),
            };
        }

        public static TestParameter From(ParameterInfo parameterInfo) => new(parameterInfo);
    }
}