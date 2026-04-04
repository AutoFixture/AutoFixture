using System;
using AutoFixture.Kernel;

namespace AutoFixtureUnitTest.Kernel;

internal class DelegatingSpecimenBuilder : ISpecimenBuilder
{
    public DelegatingSpecimenBuilder()
    {
        OnCreate = (r, c) => null;
    }

    public object Create(object request, ISpecimenContext container)
    {
        return OnCreate(request, container);
    }

    internal Func<object, ISpecimenContext, object> OnCreate { get; set; }
}