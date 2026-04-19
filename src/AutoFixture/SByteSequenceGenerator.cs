using System;
using AutoFixture.Kernel;

namespace AutoFixture;

/// <summary>
/// Creates a sequence of consecutive numbers, starting at 1.
/// </summary>
public class SByteSequenceGenerator : ISpecimenBuilder
{
    private sbyte _s;
    private readonly object _syncRoot;

    /// <summary>
    /// Initializes a new instance of the <see cref="SByteSequenceGenerator"/> class.
    /// </summary>
    public SByteSequenceGenerator()
    {
        _syncRoot = new object();
    }

    /// <summary>
    /// Creates an anonymous <see cref="sbyte"/>.
    /// </summary>
    /// <param name="request">The request that describes what to create.</param>
    /// <param name="context">Not used.</param>
    /// <returns>
    /// The next <see cref="sbyte"/> in a consecutive sequence, if <paramref name="request"/>
    /// is a request for an SByte; otherwise, a <see cref="NoSpecimen"/> instance.
    /// </returns>
    public object Create(object request, ISpecimenContext context)
    {
        if (!typeof(sbyte).Equals(request))
        {
            return NoSpecimen.Instance;
        }

        lock (_syncRoot)
        {
            return ++_s;
        }
    }
}