using System;
using System.Linq;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// A specification that evaluates whether a request is a request for a type without public constructors.
    /// </summary>
    public class NoConstructorsSpecification : IRequestSpecification
    {
        private readonly IMethodQuery modestConstructorQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="NoConstructorsSpecification"/> class.
        /// </summary>
        public NoConstructorsSpecification()
        {
            this.modestConstructorQuery = new ModestConstructorQuery();
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
            if (request == null) throw new ArgumentNullException(nameof(request));

            var type = request as Type;
            return type != null && !this.modestConstructorQuery.SelectMethods(type).Any();
        }
    }
}