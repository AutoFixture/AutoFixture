using System;
using System.Reflection;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// A specification that evaluates whether a request is a request for an abstract type such as an abstract base class or an interface.
    /// </summary>
    public class AbstractTypeSpecification : IRequestSpecification
    {
        /// <summary>
        /// Evaluates a request for a specimen.
        /// </summary>
        /// <param name="request">The specimen request.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="request"/> is a <see cref="Type"/> that represents an abstract base class or an interface;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsSatisfiedBy(object request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            return request is Type type && type.GetTypeInfo().IsAbstract;
        }
    }
}