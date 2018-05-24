using System;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Tracks any request and passes it on to the container.
    /// Tracks any returned object from the container and passes it on.
    /// </summary>
    public class TracingBuilder : ISpecimenBuilder
    {
        private IRequestSpecification filter;
        private int depth;

        /// <summary>
        /// Raised when a specimen is requested.
        /// </summary>
        public event EventHandler<RequestTraceEventArgs> SpecimenRequested;

        /// <summary>
        /// Raised when a specimen was created.
        /// </summary>
        public event EventHandler<SpecimenCreatedEventArgs> SpecimenCreated;

        /// <summary>
        /// Initializes a new instance of the <see cref="TracingBuilder"/> class with a decorated
        /// <see cref="ISpecimenBuilder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="ISpecimenBuilder"/> to decorate.</param>
        public TracingBuilder(ISpecimenBuilder builder)
        {
            this.Builder = builder ?? throw new ArgumentNullException(nameof(builder));
            this.filter = new TrueRequestSpecification();
        }

        /// <summary>
        /// Gets the builder decorated by this instance.
        /// </summary>
        public ISpecimenBuilder Builder { get; }

        /// <summary>
        /// Gets or sets a filter for tracking.
        /// </summary>
        /// <remarks>
        /// <para>
        /// By default, <see cref="Filter"/> tracks all requests and created Specimens, but you can
        /// provide a custom filter to only allow certain requests to be traced.
        /// </para>
        /// <para>
        /// As this is a variation of the Specification pattern, the filter must return
        /// <see langword="true"/> to allow the request to be tracked.
        /// </para>
        /// </remarks>
        public IRequestSpecification Filter
        {
            get => this.filter;
            set => this.filter = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Creates a new specimen based on a request and raises events tracing the progress.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// The requested specimen if possible; otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The <paramref name="request"/> can be any object, but will often be a
        /// <see cref="Type"/> or other <see cref="System.Reflection.MemberInfo"/> instances.
        /// </para>
        /// </remarks>
        public object Create(object request, ISpecimenContext context)
        {
            var isFilterSatisfied = this.filter.IsSatisfiedBy(request);
            if (isFilterSatisfied)
            {
                this.OnSpecimenRequested(new RequestTraceEventArgs(request, ++this.depth));
            }

            bool specimenWasCreated = false;
            object specimen = null;
            try
            {
                specimen = this.Builder.Create(request, context);
                specimenWasCreated = true;
                return specimen;
            }
            finally
            {
                if (isFilterSatisfied)
                {
                    if (specimenWasCreated)
                    {
                        this.OnSpecimenCreated(new SpecimenCreatedEventArgs(request, specimen, this.depth));
                    }
                    this.depth--;
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="SpecimenCreated"/> event.
        /// </summary>
        /// <param name="e">The event arguments for the event.</param>
        protected virtual void OnSpecimenCreated(SpecimenCreatedEventArgs e)
        {
            this.SpecimenCreated?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="SpecimenRequested"/> event.
        /// </summary>
        /// <param name="e">The event arguments for the event.</param>
        protected virtual void OnSpecimenRequested(RequestTraceEventArgs e)
        {
            this.SpecimenRequested?.Invoke(this, e);
        }
    }
}