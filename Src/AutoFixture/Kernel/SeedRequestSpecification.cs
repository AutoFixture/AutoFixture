using System;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// A specification that evaluates seeded requests for types against a target type.
    /// </summary>
    public class SeedRequestSpecification : IRequestSpecification
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeedRequestSpecification"/> class.
        /// </summary>
        /// <param name="type">The target type.</param>
        public SeedRequestSpecification(Type type)
        {
            this.TargetType = type ?? throw new ArgumentNullException(nameof(type));
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
        /// <see langword="true"/> if <paramref name="request"/> is a <see cref="SeededRequest"/>
        /// for a type that matches <see cref="TargetType"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsSatisfiedBy(object request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            return request is SeededRequest seededRequest &&
                   this.TargetType.Equals(seededRequest.Request);
        }
    }
}
