using System;
using System.Collections.Generic;

namespace AutoFixture.Kernel;

/// <summary>
/// Decorates <see cref="ISpecimenBuilder"/> instances with <see cref="DisposableTracker"/>
/// instances.
/// </summary>
/// <seealso cref="DisposableTracker"/>
public class DisposableTrackingBehavior : ISpecimenBuilderTransformation, IDisposable
{
    private readonly List<DisposableTracker> _trackers;

    /// <summary>
    /// Initializes a new instance of the <see cref="DisposableTrackingBehavior"/> class.
    /// </summary>
    public DisposableTrackingBehavior()
    {
        _trackers = new List<DisposableTracker>();
    }

    /// <summary>
    /// Gets the trackers created by this instance. Each time <see cref="Transform"/> is
    /// invoked, a new <see cref="DisposableTracker"/> instance is created and added to this
    /// list.
    /// </summary>
    public IEnumerable<DisposableTracker> Trackers => _trackers;

    /// <summary>
    /// Decorates the supplied builder with a <see cref="DisposableTracker"/>.
    /// </summary>
    /// <param name="builder">The builder to transform.</param>
    /// <returns>
    /// A new <see cref="DisposableTracker"/> that decorates <paramref name="builder"/>.
    /// </returns>
    /// <remarks>
    /// <para>
    /// The returned <see cref="DisposableTracker"/> is also added to the
    /// <see cref="Trackers"/> property.
    /// </para>
    /// </remarks>
    public ISpecimenBuilderNode Transform(ISpecimenBuilder builder)
    {
        var tracker = new DisposableTracker(builder);
        _trackers.Add(tracker);
        return tracker;
    }

    /// <summary>
    /// Disposes all <see cref="Trackers"/> and clears the list.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes all <see cref="Trackers"/> and clears the list.
    /// </summary>
    /// <param name="disposing">
    /// <see langword="true"/> to release both managed and unmanaged resources;
    /// <see langword="false"/> to release only unmanaged resources.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            foreach (var t in Trackers)
            {
                t.Dispose();
            }
            _trackers.Clear();
        }
    }
}