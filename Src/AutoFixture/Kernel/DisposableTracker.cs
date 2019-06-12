using System;
using System.Collections.Generic;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Tracks all disposable specimens created by a decorated <see cref="ISpecimenBuilder"/> to
    /// be able to dispose them when signalled.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The DisposableTracker examines all specimens created by a decorated
    /// <see cref="ISpecimenBuilder"/>. All specimens that implement <see cref="IDisposable"/> are
    /// tracked so that they can be deterministically disposed. This happens when
    /// <see cref="Dispose()"/> is invoked on the instance.
    /// </para>
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "The main responsibility of this class isn't to be a 'collection' (which, by the way, it isn't - it's just an Iterator).")]
    public class DisposableTracker : ISpecimenBuilderNode, IDisposable
    {
        private readonly HashSet<IDisposable> disposables;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableTracker"/> class.
        /// </summary>
        /// <param name="builder">The decorated builder.</param>
        /// <remarks>
        /// <para>
        /// After initialization, <paramref name="builder"/> is available through the
        /// <see cref="Builder"/> property.
        /// </para>
        /// </remarks>
        public DisposableTracker(ISpecimenBuilder builder)
        {
            this.Builder = builder ?? throw new ArgumentNullException(nameof(builder));
            this.disposables = new HashSet<IDisposable>();
        }

        /// <summary>
        /// Gets the decorated builder.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property exposes the decorated builder originally supplied to the constructor.
        /// </para>
        /// </remarks>
        /// <seealso cref="DisposableTracker(ISpecimenBuilder)"/>
        public ISpecimenBuilder Builder { get; }

        /// <summary>
        /// Gets the disposable specimens currently tracked by this instance.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Items are added to this list by the <see cref="Create(object, ISpecimenContext)"/>
        /// method.
        /// </para>
        /// </remarks>
        public IEnumerable<IDisposable> Disposables => this.disposables;

        /// <summary>
        /// Creates a new specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// The specimen created by the decorated builder.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method delegates creation of specimens to the decorated <see cref="Builder"/>.
        /// However, before specimens are returned they are examined and tracked in the
        /// <see cref="Disposables"/> list if they implement <see cref="IDisposable"/>. They can
        /// subsequently be disposed by invoking the <see cref="Dispose()"/> method on the
        /// instance.
        /// </para>
        /// </remarks>
        public object Create(object request, ISpecimenContext context)
        {
            var specimen = this.Builder.Create(request, context);
            if (specimen is IDisposable d)
            {
                this.disposables.Add(d);
            }
            return specimen;
        }

        /// <summary>Composes the supplied builders.</summary>
        /// <param name="builders">The builders to compose.</param>
        /// <returns>
        /// A new <see cref="ISpecimenBuilderNode" /> instance containing
        /// <paramref name="builders" /> as child nodes.
        /// </returns>
        public virtual ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
        {
            if (builders == null) throw new ArgumentNullException(nameof(builders));

            var composedBuilder = CompositeSpecimenBuilder.ComposeIfMultiple(builders);
            var d = new DisposableTracker(composedBuilder);
            this.disposables.Add(d);
            return d;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator{ISpecimenBuilder}" /> that can be used to
        /// iterate through the collection.
        /// </returns>
        public IEnumerator<ISpecimenBuilder> GetEnumerator()
        {
            yield return this.Builder;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Disposes all items in the <see cref="Disposables"/> list and removes them from the
        /// list.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes all items in the <see cref="Disposables"/> list and removes them from the
        /// list.
        /// </summary>
        /// <param name="disposing">
        /// <see langword="true"/> to release both managed and unmanaged resources;
        /// <see langword="false"/> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var d in this.Disposables)
                {
                    d.Dispose();
                }
                this.disposables.Clear();
            }
        }
    }
}
