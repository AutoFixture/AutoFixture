namespace Ploeh.AutoFixture
{
    using System;
    using System.Collections;
    using Kernel;

    /// <summary>
    /// Allows interception of requests to override Create with a specified specimen.
    /// </summary>
    public class InterceptingBuilder : ISpecimenBuilder
    {
        private Hashtable oneTimeInterceptions;
        private ISpecimenBuilder builder;

        /// <summary>
        /// Initializes a new instance of the <see cref="InterceptingBuilder"/> class.
        /// </summary>
        /// <param name="builder">The <see cref="ISpecimenBuilder"/> to decorate.</param>
        public InterceptingBuilder(ISpecimenBuilder builder)
        {
            oneTimeInterceptions = new Hashtable();
            this.builder = builder;
        }

        /// <summary>
        /// Sets an interception that will execute one time and then remove itself.
        /// </summary>
        /// <param name="requestToIntercept">The request to intercept.</param>
        /// <param name="specimen">The specimen to return.</param>
        public void SetOnetimeInterception(object requestToIntercept, object specimen)
        {
            if (oneTimeInterceptions.Contains(requestToIntercept))
            {
                oneTimeInterceptions[requestToIntercept] = specimen;
            }
            else
            {
                oneTimeInterceptions.Add(requestToIntercept, specimen);
            }
        }

        /// <summary>
        /// Creates a new specimen based on a request, using any interceptions defined.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="container">A container that can be used to create other specimens.</param>
        /// <returns>
        /// The requested specimen if possible; otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The <paramref name="request"/> can be any object, but will often be a
        /// <see cref="Type"/> or other <see cref="System.Reflection.MemberInfo"/> instances.
        /// </para>
        /// </remarks>
        public object Create(object request, ISpecimenContainer container)
        {
            if (oneTimeInterceptions.Contains(request))
            {
                object intercept = oneTimeInterceptions[request];
                oneTimeInterceptions.Remove(request);
                return intercept;
            }

            return builder.Create(request, container);
        }
    }
}