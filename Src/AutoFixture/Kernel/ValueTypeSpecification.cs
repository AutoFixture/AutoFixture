using System;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// A specification that evaluates whether a request is a request for a value type such as a structure.
    /// </summary>
    public class ValueTypeSpecification : IRequestSpecification
    {

        /// <summary>
        /// Evaluates a request for a specimen.
        /// </summary>
        /// <param name="request">The specimen request.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="request"/> is a <see cref="Type"/> that represents a structure;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsSatisfiedBy(object request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            var type = request as Type;
            return type != null && type.IsValueTypeButNotPrimitiveOrEnum();
        }
    }
}