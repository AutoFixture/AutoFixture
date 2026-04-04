using System;
using System.Linq;
using System.Reflection;

namespace AutoFixture.Xunit2.Internal;

internal class TestParameter
{
    private readonly Lazy<ICustomization> _lazyCustomization;
    private readonly Lazy<FrozenAttribute> _lazyFrozenAttribute;

    public TestParameter(ParameterInfo parameterInfo)
    {
        ParameterInfo = parameterInfo ?? throw new ArgumentNullException(nameof(parameterInfo));

        _lazyCustomization = new Lazy<ICustomization>(
            () => GetCustomization(parameterInfo));
        _lazyFrozenAttribute = new Lazy<FrozenAttribute>(
            () => parameterInfo.GetCustomAttributes()
                .OfType<FrozenAttribute>().FirstOrDefault());
    }

    public ParameterInfo ParameterInfo { get; }

    public ICustomization GetCustomization() => _lazyCustomization.Value;

    public ICustomization GetCustomization(object value)
    {
        var frozenAttribute = _lazyFrozenAttribute.Value;

        if (frozenAttribute is null)
            return NullCustomization.Instance;

        return new FrozenValueCustomization(
            new ParameterFilter(ParameterInfo, frozenAttribute.By),
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
            { Length: 0 } => NullCustomization.Instance,
            { Length: 1 } => customizations[0],
            _ => new CompositeCustomization(customizations),
        };
    }

    public static TestParameter From(ParameterInfo parameterInfo) => new(parameterInfo);
}