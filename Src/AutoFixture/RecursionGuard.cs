namespace Ploeh.AutoFixture
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Kernel;

    /// <summary>
    /// Base class for recursion handling. Tracks requests and reacts when a recursion point in the
    /// specimen creation process is detected.
    /// </summary>
	public abstract class RecursionGuard : ISpecimenBuilder
	{
        private readonly ISpecimenBuilder builder;
        private readonly IEqualityComparer comparer;
		private readonly Stack<object> monitoredRequests;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecursionGuard"/> class.
        /// </summary>
        /// <param name="builder">The intercepting builder to decorate.</param>
        /// <param name="comparer">
        /// An IEqualitycomparer implementation to use when comparing requests to determine recursion.
        /// </param>
        protected RecursionGuard(ISpecimenBuilder builder, IEqualityComparer comparer)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }

            monitoredRequests = new Stack<object>();
            this.builder = builder;
            this.comparer = comparer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecursionGuard"/> class.
        /// </summary>
        /// <param name="builder">The intercepting builder to decorate.</param>
        protected RecursionGuard(ISpecimenBuilder builder) : this(builder, EqualityComparer<object>.Default)
        {
        }

        /// <summary>
        /// Gets the recorded requests so far.
        /// </summary>
        /// <value>The recorded requests.</value>
        protected object[] GetRecordedRequests()
        {
            return monitoredRequests.ToArray();
        }

        /// <summary>
        /// Handles a request that would cause recursion.
        /// </summary>
        /// <param name="request">The recursion causing request.</param>
        /// <returns>The specimen to return.</returns>
        public abstract object HandleRecursiveRequest(object request);

        /// <summary>
        /// Creates a new specimen based on a request.
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
        public object Create(object request, ISpecimenContext container)
        {
            if (monitoredRequests.Any(x => comparer.Equals(x, request)))
            {
                return HandleRecursiveRequest(request);
            }

            monitoredRequests.Push(request);
            object specimen = this.builder.Create(request, container);
            monitoredRequests.Pop();
            return specimen;
        }
	}
}