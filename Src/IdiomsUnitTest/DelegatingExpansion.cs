using System;
using System.Collections.Generic;
using AutoFixture.Idioms;

namespace AutoFixture.IdiomsUnitTest;

public class DelegatingExpansion<T> : IExpansion<T>
{
    public DelegatingExpansion()
    {
        OnExpand = v => new[] { v };
    }

    public Func<T, IEnumerable<T>> OnExpand { get; set; }

    public IEnumerable<T> Expand(T value)
    {
        return OnExpand(value);
    }
}