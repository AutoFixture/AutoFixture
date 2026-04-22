using System;
using AutoFixture.Kernel;

namespace AutoFixture;

/// <summary>
/// Creates a sequence of consecutive numbers, starting at 1.
/// </summary>
public class UInt32SequenceGenerator : ISpecimenBuilder
{
    private readonly object _syncRoot;
    private uint _u;

    /// <summary>
    /// Initializes a new instance of the <see cref="UInt32SequenceGenerator"/> class.
    /// </summary>
    public UInt32SequenceGenerator()
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
    /// for an unsigned integer; otherwise, a <see cref="NoSpecimen"/> instance.
    /// </returns>
    public object Create(object request, ISpecimenContext context)
    {
        if (!typeof(uint).Equals(request))
        {
            return NoSpecimen.Instance;
        }

        lock (_syncRoot)
        {
            return ++_u;
        }
    }
}