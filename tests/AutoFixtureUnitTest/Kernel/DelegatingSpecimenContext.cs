using System;
using AutoFixture.Kernel;

namespace AutoFixtureUnitTest.Kernel;

internal class DelegatingSpecimenContext : ISpecimenContext
{
    public DelegatingSpecimenContext()
    {
        OnResolve = r => null;
    }

    public object Resolve(object request)
    {
        return OnResolve(request);
    }

    internal Func<object, object> OnResolve { get; set; }
}