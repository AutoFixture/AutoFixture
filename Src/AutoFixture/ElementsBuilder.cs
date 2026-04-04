using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Kernel;

namespace AutoFixture;

/// <summary>
/// Draws a random element from the given collection.
/// </summary>
public sealed class ElementsBuilder<T> : ISpecimenBuilder
{
    private readonly T[] _elements;
    private readonly RandomNumericSequenceGenerator _sequence;

    /// <summary>
    /// Initializes a new instance of the <see cref="ElementsBuilder{T}"/> class.
    /// </summary>
    public ElementsBuilder(params T[] elements)
        : this(elements.AsEnumerable())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ElementsBuilder{T}"/> class.
    /// </summary>
    public ElementsBuilder(IEnumerable<T> elements)
    {
        if (elements == null) throw new ArgumentNullException(nameof(elements));

        _elements = elements.ToArray();

        if (_elements.Length < 1)
        {
            throw new ArgumentException(
                "The supplied collection of elements must contain at least one element. " +
                "This collection is expected to contain the elements from which the randomized algorithm will draw; " +
                "if the collection is empty, there are no elements to draw.",
                nameof(elements));
        }

        // The RandomNumericSequenceGenerator is only created for collections of minimum 2 elements
        if (_elements.Length > 1)
            _sequence = new RandomNumericSequenceGenerator(0, _elements.Length - 1);
    }

    /// <summary>
    /// Returns one of the element present in the collection given when the object was constructed.
    /// </summary>
    /// <param name="request">The request that describes what to create.</param>
    /// <param name="context">Not used.</param>
    /// <returns>
    /// One of the element present in the collection given to the constructor if <paramref name="request"/>
    /// is a request for <typeparamref name="T"/>; otherwise, a <see cref="NoSpecimen"/> instance.
    /// </returns>
    public object Create(object request, ISpecimenContext context)
    {
        if (!typeof(T).Equals(request))
            return NoSpecimen.Instance;

        return _elements[GetNextIndex()];
    }

    private int GetNextIndex()
    {
        if (_elements.Length == 1)
            return 0;
        else
            return (int)_sequence.Create(typeof(int));
    }
}