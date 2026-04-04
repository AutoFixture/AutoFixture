using System;

namespace AutoFixtureUnitTest.Kernel;

public class DelegatingCriterion<T> : IEquatable<T>
{
    public DelegatingCriterion()
    {
        OnEquals = _ => false;
    }

    public bool Equals(T other)
    {
        return OnEquals(other);
    }

    public Func<T, bool> OnEquals { get; set; }
}