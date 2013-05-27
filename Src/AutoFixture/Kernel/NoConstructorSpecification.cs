using System;
using System.Linq;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// A specification that evaluates whether a request is a request for a type without public constructors.
    /// </summary>
    public class NoConstructorSpecification : IRequestSpecification
    {
        private readonly IMethodQuery modestConstructorQuery;

        public NoConstructorSpecification()
        {
            modestConstructorQuery = new ModestConstructorQuery();
        }

        /// <summary>
        /// Evaluates a request for a specimen.
        /// </summary>
        /// <param name="request">The specimen request.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="request"/> is a <see cref="Type"/> that represents a type without public constructors;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsSatisfiedBy(object request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            var type = request as Type;
            return type != null && !modestConstructorQuery.SelectMethods(type).Any();
        }
    }
}