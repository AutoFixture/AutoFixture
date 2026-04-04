#nullable enable
using System;
using AutoFixture.Kernel;

namespace AutoFixture.Xunit3.Internal;

internal class FrozenValueCustomization : ICustomization
{
    private readonly IRequestSpecification _specification;
    private readonly object? _value;

    public FrozenValueCustomization(IRequestSpecification specification, object? value)
    {
        _specification = specification ?? throw new ArgumentNullException(nameof(specification));
        _value = value;
    }

    public void Customize(IFixture fixture)
    {
        var builder = new FilteringSpecimenBuilder(
            builder: new FixedBuilder(_value),
            specification: _specification);

        fixture.Customizations.Insert(0, builder);
    }
}