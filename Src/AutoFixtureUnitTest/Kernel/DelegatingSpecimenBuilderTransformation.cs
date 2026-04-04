using System;
using AutoFixture.Kernel;

namespace AutoFixtureUnitTest.Kernel;

public class DelegatingSpecimenBuilderTransformation : ISpecimenBuilderTransformation
{
    public DelegatingSpecimenBuilderTransformation()
    {
        OnTransform = b => (ISpecimenBuilderNode)b;
    }

    public ISpecimenBuilderNode Transform(ISpecimenBuilder builder)
    {
        return OnTransform(builder);
    }

    internal Func<ISpecimenBuilder, ISpecimenBuilderNode> OnTransform { get; set; }
}