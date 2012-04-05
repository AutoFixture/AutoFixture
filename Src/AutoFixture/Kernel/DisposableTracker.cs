using System;
using System.Collections.Generic;

namespace Ploeh.AutoFixture.Kernel
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
    public class DisposableTracker : ISpecimenBuilderNode, IDisposable
    {
        private readonly ISpecimenBuilder builder;
        private readonly HashSet<IDisposable> disposables;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableTracker"/> class.
        /// </summary>
        /// <param name="builder">The decorated builder.</param>
        /// <remarks>
        /// <para>
        /// After initilization, <paramref name="builder"/> is availble through the
        /// <see cref="Builder"/> property.
        /// </para>
        /// </remarks>
        public DisposableTracker(ISpecimenBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }

            this.builder = builder;
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
        public ISpecimenBuilder Builder
        {
            get { return this.builder; }
        }

        /// <summary>
        /// Gets the disposable specimens currently tracked by this instance.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Items are added to this list by the <see cref="Create(object, ISpecimenContext)"/>
        /// method.
        /// </para>
        /// </remarks>
        public IEnumerable<IDisposable> Disposables
        {
            get { return this.disposables; }
        }

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
            var d = specimen as IDisposable;
            if (d != null)
            {
                this.disposables.Add(d);
            }
            return specimen;
        }

        public virtual ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
        {
            var composedBuilder = CompositeSpecimenBuilder.ComposeIfMultiple(builders);
            return new DisposableTracker(composedBuilder);
        }

        public IEnumerator<ISpecimenBuilder> GetEnumerator()
        {
            yield return this.builder;
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
                foreach(var d in this.Disposables)
                {
                    d.Dispose();
                }
                this.disposables.Clear();
            }
        }
    }
}
