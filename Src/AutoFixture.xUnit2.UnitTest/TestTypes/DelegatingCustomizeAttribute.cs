using System;
using System.Reflection;

namespace AutoFixture.Xunit2.UnitTest.TestTypes;

internal class DelegatingCustomizeAttribute : CustomizeAttribute
{
    public DelegatingCustomizeAttribute()
    {
        OnGetCustomization = p => new DelegatingCustomization();
    }

    public override ICustomization GetCustomization(ParameterInfo parameter)
    {
        return OnGetCustomization(parameter);
    }

    public Func<ParameterInfo, ICustomization> OnGetCustomization { get; set; }
}