using System;
using System.Threading;
using AutoFixture.Kernel;

namespace AutoFixture;

/// <summary>
/// Creates a sequence of consecutive numbers, starting at 1.
/// </summary>
public class Int32SequenceGenerator : ISpecimenBuilder
{
    private int _i;

    /// <summary>
    /// Creates an anonymous number.
    /// </summary>
    /// <param name="request">The request that describes what to create.</param>
    /// <param name="context">Not used.</param>
    /// <returns>
    /// The next number in a consecutive sequence, if <paramref name="request"/> is a request
    /// for an integer; otherwise, a <see cref="NoSpecimen"/> instance.
    /// </returns>
    public object Create(object request, ISpecimenContext context)
    {
        if (!typeof(int).Equals(request))
        {
            return NoSpecimen.Instance;
        }

        return Interlocked.Increment(ref _i);
    }
}