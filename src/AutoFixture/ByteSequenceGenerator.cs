using System;
using AutoFixture.Kernel;

namespace AutoFixture;

/// <summary>
/// Creates a sequence of consecutive numbers, starting at 1.
/// </summary>
public class ByteSequenceGenerator : ISpecimenBuilder
{
    private byte _b;
    private readonly object _syncRoot;

    /// <summary>
    /// Initializes a new instance of the <see cref="ByteSequenceGenerator"/> class.
    /// </summary>
    public ByteSequenceGenerator()
    {
        _syncRoot = new object();
    }

    /// <summary>
    /// Creates an anonymous byte.
    /// </summary>
    /// <param name="request">The request that describes what to create.</param>
    /// <param name="context">Not used.</param>
    /// <returns>
    /// The next byte in a consecutive sequence, if <paramref name="request"/> is a request
    /// for a byte; otherwise, a <see cref="NoSpecimen"/> instance.
    /// </returns>
    public object Create(object request, ISpecimenContext context)
    {
        if (!typeof(byte).Equals(request))
        {
            return NoSpecimen.Instance;
        }

        lock (_syncRoot)
        {
            return ++_b;
        }
    }
}