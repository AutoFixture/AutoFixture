#if NET6_0_OR_GREATER

using System;
using System.Reflection;
using AutoFixture.Kernel;

namespace AutoFixture;

/// <summary>
/// Creates random <see cref="TimeOnly"/> specimens.
/// </summary>
/// <remarks>
/// The generated <see cref="TimeOnly"/> values will be within
/// a range of ± six hours from noon,
/// unless a different range has been specified in the constructor.
/// </remarks>
public class RandomTimeOnlySequenceGenerator : ISpecimenBuilder
{
    private static readonly TimeOnly Default = new(12, 0);
    private readonly RandomNumericSequenceGenerator randomizer;

    /// <summary>
    /// Initializes a new instance of the <see cref="RandomTimeOnlySequenceGenerator"/> class.
    /// </summary>
    public RandomTimeOnlySequenceGenerator()
        : this(Default.AddHours(-6), Default.AddHours(6))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RandomTimeOnlySequenceGenerator"/> class
    /// for a specific range of time.
    /// </summary>
    /// <param name="minTime">The lower bound of the time range.</param>
    /// <param name="maxTime">The upper bound of the time range.</param>
    /// <exception cref="ArgumentException">
    /// <paramref name="minTime"/> is greater than <paramref name="maxTime"/>.
    /// </exception>
    public RandomTimeOnlySequenceGenerator(TimeOnly minTime, TimeOnly maxTime)
    {
        if (minTime >= maxTime)
        {
            throw new ArgumentException("The 'minTime' argument must be less than the 'maxTime'.");
        }

        this.randomizer = new RandomNumericSequenceGenerator(minTime.Ticks, maxTime.Ticks);
    }

    /// <summary>
    /// Creates a new <see cref="TimeOnly"/> specimen based on a request.
    /// </summary>
    /// <param name="request">The request that describes what to create.</param>
    /// <param name="context">Not used.</param>
    /// <returns>
    /// A new <see cref="TimeOnly"/> specimen, if <paramref name="request"/> is a request for a
    /// <see cref="TimeOnly"/> value; otherwise, a <see cref="NoSpecimen"/> instance.
    /// </returns>
    public object Create(object request, ISpecimenContext context)
    {
        if (context is null) throw new ArgumentNullException(nameof(context));

        if (!typeof(TimeOnly).GetTypeInfo().IsAssignableFrom(request as Type))
        {
            return new NoSpecimen();
        }

        var ticks = (long)this.randomizer.Create(typeof(long), context);
        return new TimeOnly(ticks);
    }
}
#endif
