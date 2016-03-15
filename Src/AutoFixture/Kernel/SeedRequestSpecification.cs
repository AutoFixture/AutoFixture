using System;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// A specification that evaluates seeded requests for types against a target type.
    /// </summary>
    public class SeedRequestSpecification : IRequestSpecification
    {
        private readonly Type targetType;

        /// <summary>
        /// Initializes a new instance of the <see cref="SeedRequestSpecification"/> class.
        /// </summary>
        /// <param name="type">The target type.</param>
        public SeedRequestSpecification(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            this.targetType = type;
        }

        /// <summary>
        /// Gets the type targeted by this <see cref="IRequestSpecification"/>.
        /// </summary>
        public Type TargetType
        {
            get { return this.targetType; }
        }

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
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var sr = request as SeededRequest;
            if (sr != null)
            {
                return this.TargetType.Equals(sr.Request);
            }

            return false;
        }
    }
}
