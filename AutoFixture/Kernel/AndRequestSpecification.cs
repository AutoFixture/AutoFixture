using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// A boolean 'And' Composite <see cref="IRequestSpecification"/>.
    /// </summary>
    public class AndRequestSpecification : IRequestSpecification
    {
        private readonly IEnumerable<IRequestSpecification> specifications;

        /// <summary>
        /// Initializes a new instance of the <see cref="AndRequestSpecification"/> with the
        /// supplied specifications.
        /// </summary>
        /// <param name="specifications">An array of <see cref="IRequestSpecification"/>.</param>
        public AndRequestSpecification(params IRequestSpecification[] specifications)
        {
            if (specifications == null)
            {
                throw new ArgumentNullException("specifications");
            }

            this.specifications = specifications;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AndRequestSpecification"/> class with the
        /// supplied specifications.
        /// </summary>
        /// <param name="specifications">A sequence of <see cref="IRequestSpecification"/>.</param>
        public AndRequestSpecification(IEnumerable<IRequestSpecification> specifications)
            : this(specifications.ToArray())
        {
        }

        /// <summary>
        /// Gets the decorated specifications.
        /// </summary>
        public IEnumerable<IRequestSpecification> Specifications
        {
            get { return this.specifications; }
        }

        #region IRequestSpecification Members

        /// <summary>
        /// Evaluates a request for a specimen.
        /// </summary>
        /// <param name="request">The specimen request.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="request"/> is satisfied by all
        /// <see cref="Specifications"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsSatisfiedBy(object request)
        {
            return this.specifications.All(s => s.IsSatisfiedBy(request));
        }

        #endregion
    }
}
