namespace Ploeh.AutoFixture
{
    using System;
    using System.Collections.Generic;
	using Kernel;

    /// <summary>
    /// Base class for recursion handling. Tracks requests and reacts when a recursion point in the
    /// specimen creation process is detected.
    /// </summary>
	public class RecursionCatcher : ISpecimenBuilder
	{
        private readonly ISpecimenBuilder builder;
		private Stack<object> monitoredRequests;
        private Func<object, object> interceptRecursionRequest;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecursionCatcher"/> class.
        /// </summary>
        /// <param name="builder">The intercepting builder to decorate.</param>
        public RecursionCatcher(ISpecimenBuilder builder)
        {
            monitoredRequests = new Stack<object>();
            this.builder = builder;
        }

        /// <summary>
        /// Gets or sets the recursion request interceptor.
        /// The recursion request interceptor is called for any recursion-causing requests.
        /// </summary>
        /// <value>The recursion request interceptor.</value>
        public Func<object, object> RecursionRequestInterceptor
        {
            get { return this.interceptRecursionRequest; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                this.interceptRecursionRequest = value;
            }
        }

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
        public object Create(object request, ISpecimenContainer container)
        {
            if (monitoredRequests.Contains(request))
            {
                return interceptRecursionRequest(request);
            }

            monitoredRequests.Push(request);

            object specimen = this.builder.Create(request, container);
            monitoredRequests.Pop();
            return specimen;
        }
	}
}