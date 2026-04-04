using System;
using AutoFixture;

namespace AutoFixtureUnitTest;

internal class DelegatingCustomization : ICustomization
{
    public DelegatingCustomization()
    {
        OnCustomize = f => { };
    }

    public void Customize(IFixture fixture)
    {
        OnCustomize(fixture);
    }

    internal Action<IFixture> OnCustomize { get; set; }
}