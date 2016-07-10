using System;
using System.Reflection;
using Ploeh.Albedo;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Relays a request for an <see cref="ParameterInfoElement" /> to a request for a
    /// <see cref="ParameterInfo"/> and returns the result.
    /// </summary>
    public class ParameterInfoElementRelay : ISpecimenBuilder
    {
        /// <summary>
        /// Creates a new specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// A specimen resolved from <paramref name="context"/> if possible; otherwise
        /// a <see cref="NoSpecimen"/> instance.
        /// </returns>
        /// <remarks>
        /// <para>
        /// If <paramref name="request"/> is a request for an <see cref="ParameterInfoElement"/>,
        /// <paramref name="context"/> is used to create and return a specimen using request's 
        /// <see cref="ParameterInfo"/>. If not, the return value is a <see cref="NoSpecimen"/>.
        /// instance.
        /// </para>
        /// </remarks>
        public object Create(object request, ISpecimenContext context)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var reflectionElement = request as ParameterInfoElement;
            if (reflectionElement == null)
            {
                return new NoSpecimen();
            }

            return context.Resolve(reflectionElement.ParameterInfo);
        }
    }
}