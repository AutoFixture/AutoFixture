using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Tracks any request and passes it on to the container.
    /// Tracks any returned object from the container and passes it on.
    /// </summary>
    public abstract class RequestTracker : ISpecimenBuilder
    {
        private readonly ISpecimenBuilder builder;
        private Func<object, bool> shouldTrack;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestTracker"/> class with a decorated
        /// <see cref="ISpecimenBuilder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="ISpecimenBuilder"/> to decorate.</param>
        protected RequestTracker(ISpecimenBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }

            this.builder = builder;
            this.TrackSpecification = r => true;
        }

        /// <summary>
        /// Gets or sets a filter for tracking
        /// </summary>
        /// <remarks>
        /// <para>
        /// By default, <see cref="TrackSpecification"/> tracks all requests and created Specimens,
        /// but you can provide a custom filter to only allow certain requests to be tracked.
        /// </para>
        /// <para>
        /// As this is a variation of the Specification pattern, the filter must return
        /// <see langword="true"/> to allow the request to be tracked.
        /// </para>
        /// </remarks>
        public Func<object, bool> TrackSpecification
        {
            get { return this.shouldTrack; }
            set 
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                this.shouldTrack = value;
            }
        }

        /// <summary>
        /// Tracks the specimen creation request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="container">A container that can be used to create other specimens.</param>
        /// <returns>
        /// The requested specimen if possible; otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        /// <remarks>
        /// 	<para>
        /// The <paramref name="request"/> can be any object, but will often be a
        /// <see cref="Type"/> or other <see cref="System.Reflection.MemberInfo"/> instances.
        /// </para>
        /// </remarks>
        public object Create(object request, ISpecimenContainer container)
        {
            if (this.shouldTrack(request))
            {
                this.TrackRequest(request);
            }
            
            object specimen = this.builder.Create(request, container);
            if (this.shouldTrack(request))
            {
                this.TrackCreatedSpecimen(specimen);
            }

            return specimen;
        }

        /// <summary>
        /// Invoked when a request is tracked.
        /// </summary>
        /// <param name="request">The request.</param>
        protected abstract void TrackRequest(object request);

        /// <summary>
        /// Invoked when a created specimen is tracked.
        /// </summary>
        /// <param name="specimen">The specimen.</param>
        protected abstract void TrackCreatedSpecimen(object specimen);
    }
}