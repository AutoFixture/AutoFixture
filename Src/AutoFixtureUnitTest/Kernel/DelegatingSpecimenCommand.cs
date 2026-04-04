using System;
using AutoFixture.Kernel;

namespace AutoFixtureUnitTest.Kernel;

public class DelegatingSpecimenCommand : ISpecimenCommand
{
    public DelegatingSpecimenCommand()
    {
        OnExecute = (s, c) => { };
    }

    public void Execute(object specimen, ISpecimenContext context)
    {
        OnExecute(specimen, context);
    }

    internal Action<object, ISpecimenContext> OnExecute { get; set; }
}