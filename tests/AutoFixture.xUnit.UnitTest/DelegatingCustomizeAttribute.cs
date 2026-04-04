using System;
using System.Reflection;

namespace AutoFixture.Xunit.UnitTest;

internal class DelegatingCustomizeAttribute : CustomizeAttribute
{
    internal DelegatingCustomizeAttribute()
    {
        OnGetCustomization = p => new DelegatingCustomization();
    }

    public override ICustomization GetCustomization(ParameterInfo parameter)
    {
        return OnGetCustomization(parameter);
    }

    internal Func<ParameterInfo, ICustomization> OnGetCustomization { get; set; }
}