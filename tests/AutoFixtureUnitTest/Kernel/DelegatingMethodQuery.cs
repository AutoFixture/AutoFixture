using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Kernel;

namespace AutoFixtureUnitTest.Kernel;

public class DelegatingMethodQuery : IMethodQuery
{
    public DelegatingMethodQuery()
    {
        OnSelectMethods = t => Enumerable.Empty<IMethod>();
    }

    public IEnumerable<IMethod> SelectMethods(Type type)
    {
        return OnSelectMethods(type);
    }

    internal Func<Type, IEnumerable<IMethod>> OnSelectMethods { get; set; }
}