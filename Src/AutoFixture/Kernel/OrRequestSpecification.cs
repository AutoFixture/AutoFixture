using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// A boolean 'Or' Composite <see cref="IRequestSpecification"/>.
    /// </summary>
    public class OrRequestSpecification : IRequestSpecification
    {
        private readonly IRequestSpecification[] specifications;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrRequestSpecification"/> class with the
        /// supplied specifications.
        /// </summary>
        /// <param name="specifications">An array of <see cref="IRequestSpecification"/>.</param>
        public OrRequestSpecification(params IRequestSpecification[] specifications)
        {
            this.specifications = specifications ?? throw new ArgumentNullException(nameof(specifications));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrRequestSpecification"/> class with the
        /// supplied specifications.
        /// </summary>
        /// <param name="specifications">A sequence of <see cref="IRequestSpecification"/>.</param>
        public OrRequestSpecification(IEnumerable<IRequestSpecification> specifications)
            : this(specifications.ToArray())
        {
        }

        /// <summary>
        /// Gets the decorated specifications.
        /// </summary>
        public IEnumerable<IRequestSpecification> Specifications => this.specifications;

        /// <summary>
        /// Evaluates a request for a specimen.
        /// </summary>
        /// <param name="request">The specimen request.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="request"/> is satisfied by any of the
        /// <see cref="Specifications"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsSatisfiedBy(object request)
        {
            // This is performance-sensitive code when used repeatedly over many requests.
            // See discussion at https://github.com/AutoFixture/AutoFixture/pull/218
            if (this.specifications.Length == 0) return true;
            for (int i = 0; i < this.specifications.Length; i++)
            {
                var satisfied = this.specifications[i].IsSatisfiedBy(request);
                if (satisfied) return true;
            }
            return false;
        }
    }
}
