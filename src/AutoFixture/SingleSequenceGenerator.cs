using System;
using AutoFixture.Kernel;

namespace AutoFixture;

/// <summary>
/// Creates a sequence of consecutive numbers, starting at 1.
/// </summary>
public class SingleSequenceGenerator : ISpecimenBuilder
{
    private float _f;
    private readonly object _syncRoot;

    /// <summary>
    /// Initializes a new instance of the <see cref="SingleSequenceGenerator"/> class.
    /// </summary>
    public SingleSequenceGenerator()
    {
        _syncRoot = new object();
    }

    /// <summary>
    /// Creates an anonymous number.
    /// </summary>
    /// <param name="request">The request that describes what to create.</param>
    /// <param name="context">Not used.</param>
    /// <returns>
    /// The next number in a consecutive sequence, if <paramref name="request"/> is a request
    /// for a <see cref="float"/>; otherwise, a <see cref="NoSpecimen"/> instance.
    /// </returns>
    public object Create(object request, ISpecimenContext context)
    {
        if (!typeof(float).Equals(request))
        {
            return NoSpecimen.Instance;
        }

        lock (_syncRoot)
        {
            return ++_f;
        }
    }
}