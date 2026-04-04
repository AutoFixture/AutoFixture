using System;
using System.Collections.Generic;
using System.Linq;
using Albedo;

namespace AutoFixture.Idioms;

/// <summary>
/// Allows comparing <see cref="IReflectionElement"/> instances, where the comparison
/// is implemented by collecting the elements using an <see cref="IReflectionVisitor{T}"/>
/// then comparing them using.
/// </summary>
/// <typeparam name="T"></typeparam>
internal abstract class ReflectionVisitorElementComparer<T> : IEqualityComparer<IReflectionElement>
{
    private readonly IReflectionVisitor<IEnumerable<T>> _visitor;
    private readonly IEqualityComparer<T> _comparer;

    internal ReflectionVisitorElementComparer(
        IReflectionVisitor<IEnumerable<T>> visitor,
        IEqualityComparer<T> comparer = null)
    {
        _visitor = visitor;
        _comparer = comparer ?? EqualityComparer<T>.Default;
    }

    bool IEqualityComparer<IReflectionElement>.Equals(IReflectionElement x, IReflectionElement y)
    {
        var values = new CompositeReflectionElement(x, y)
            .Accept(_visitor)
            .Value
            .ToArray();

        var distinctValues = values.Distinct(_comparer);

        return values.Length == 2
               && distinctValues.Count() == 1;
    }

    int IEqualityComparer<IReflectionElement>.GetHashCode(IReflectionElement obj)
    {
        if (obj == null) throw new ArgumentNullException(nameof(obj));
        return obj
            .Accept(_visitor)
            .Value
            .Single()
            .GetHashCode();
    }
}