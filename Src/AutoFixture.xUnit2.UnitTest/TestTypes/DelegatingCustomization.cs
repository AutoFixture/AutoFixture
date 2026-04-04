using System;

namespace AutoFixture.Xunit2.UnitTest.TestTypes;

internal class DelegatingCustomization : ICustomization
{
    internal DelegatingCustomization()
    {
        OnCustomize = _ => { };
    }

    public void Customize(IFixture fixture)
    {
        OnCustomize(fixture);
    }

    internal Action<IFixture> OnCustomize { get; set; }
}