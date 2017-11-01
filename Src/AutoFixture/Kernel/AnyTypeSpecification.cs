using System;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// A specification that evaluates whether a request is a request for a type.
    /// </summary>
    public class AnyTypeSpecification : IRequestSpecification
    {
        /// <summary>
        /// Evaluates a request for a specimen.
        /// </summary>
        /// <param name="request">The specimen request.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="request"/> is a <see cref="Type"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsSatisfiedBy(object request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            return request is Type;
        }
    }
}
