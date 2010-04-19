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
		private bool skip;

        /// <summary>
        /// Gets the types to ignore when tracking.
        /// </summary>
        /// <value>The ignored types.</value>
		public IList<Type> IgnoredTypes
		{
			get;
			private set;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestTracker"/> class.
        /// </summary>
		protected RequestTracker()
		{
			IgnoredTypes = new Collection<Type>();
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
			// Avoid tracking self
			if (skip)
			{
				skip = false;
				return new NoSpecimen(request);
			}

			if (!IgnoredTypes.Contains(request.GetType()))
				TrackRequest(request);
			skip = true;
			object specimen = container.Create(request);
			if (!IgnoredTypes.Contains(request.GetType()))
				TrackCreatedSpecimen(specimen);
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