using System;
using AutoFixture.Kernel;

namespace AutoFixture;

/// <summary>
/// Creates a sequence of consecutive numbers, starting at 1.
/// </summary>
public class DecimalSequenceGenerator : ISpecimenBuilder
{
    private decimal _d;
    private readonly object _syncRoot;

    /// <summary>
    /// Initializes a new instance of the <see cref="Int64SequenceGenerator"/> class.
    /// </summary>
    public DecimalSequenceGenerator()
    {
        _syncRoot = new object();
    }

    /// <inheritdoc />
    public object Create(object request, ISpecimenContext context)
    {
        if (!typeof(decimal).Equals(request))
        {
            return NoSpecimen.Instance;
        }

        lock (_syncRoot)
        {
            return ++_d;
        }
    }
}