using System;
using System.Reflection;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Relays a request for a parameter to a <see cref="SeededRequest"/> with a seed based
    /// on the parameter's name.
    /// </summary>
    public class ParameterRequestRelay : ISpecimenBuilder
    {
        /// <summary>
        /// Creates a specimen based on a requested parameter.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// A specimen created from a <see cref="SeededRequest"/> encapsulating the parameter type
        /// and name of the requested parameter, if possible; otherwise, a <see cref="NoSpecimen"/>
        /// instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var paramInfo = request as ParameterInfo;
            if (paramInfo == null)
            {
                return new NoSpecimen();
            }

            return context.Resolve(new SeededRequest(paramInfo.ParameterType, paramInfo.Name));
        }
    }
}
