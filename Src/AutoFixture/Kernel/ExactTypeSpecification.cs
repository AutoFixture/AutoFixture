using System;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// A specification that evaluates requests for types against a target type.
    /// </summary>
    public class ExactTypeSpecification : IRequestSpecification
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExactTypeSpecification"/> class.
        /// </summary>
        /// <param name="type">The target type.</param>
        public ExactTypeSpecification(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            this.TargetType = type;
        }

        /// <summary>
        /// Gets the type targeted by this <see cref="IRequestSpecification"/>.
        /// </summary>
        public Type TargetType { get; }

        /// <summary>
        /// Evaluates a request for a specimen.
        /// </summary>
        /// <param name="request">The specimen request.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="request"/> matches the target type;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsSatisfiedBy(object request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return this.TargetType.Equals(request);
        }
    }
}
