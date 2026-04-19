using System;
using AutoFixture.Kernel;

namespace AutoFixture;

/// <summary>
/// Creates a sequence of consecutive numbers, starting at 1.
/// </summary>
public class DoubleSequenceGenerator : ISpecimenBuilder
{
    private double _d;
    private readonly object _syncRoot;

    /// <summary>
    /// Initializes a new instance of the <see cref="DoubleSequenceGenerator"/> class.
    /// </summary>
    public DoubleSequenceGenerator()
    {
        _syncRoot = new object();
    }

    /// <inheritdoc />
    public object Create(object request, ISpecimenContext context)
    {
        if (!typeof(double).Equals(request))
        {
            return NoSpecimen.Instance;
        }

        lock (_syncRoot)
        {
            return ++_d;
        }
    }
}