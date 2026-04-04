using System;

namespace AutoFixture.Xunit.UnitTest;

internal class DelegatingCustomization : ICustomization
{
    internal DelegatingCustomization()
    {
        OnCustomize = f => { };
    }

    public void Customize(IFixture fixture)
    {
        OnCustomize(fixture);
    }

    internal Action<IFixture> OnCustomize { get; set; }
}