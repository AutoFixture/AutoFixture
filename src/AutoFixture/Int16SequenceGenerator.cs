using System;
using AutoFixture.Kernel;

namespace AutoFixture;

/// <summary>
/// Creates a sequence of consecutive numbers, starting at 1.
/// </summary>
public class Int16SequenceGenerator : ISpecimenBuilder
{
    private short _s;
    private readonly object _syncRoot;

    /// <summary>
    /// Initializes a new instance of the <see cref="Int16SequenceGenerator"/> class.
    /// </summary>
    public Int16SequenceGenerator()
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
    /// for a 16-bit integer; otherwise, a <see cref="NoSpecimen"/> instance.
    /// </returns>
    public object Create(object request, ISpecimenContext context)
    {
        if (!typeof(short).Equals(request))
        {
            return NoSpecimen.Instance;
        }

        lock (_syncRoot)
        {
            return ++_s;
        }
    }
}