﻿using System;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Decorates an <see cref="IRequestSpecification"/> and returns the opposite result.
    /// </summary>
    public class InverseRequestSpecification : IRequestSpecification
    {
        private readonly IRequestSpecification spec;

        /// <summary>
        /// Initializes a new instance of the <see cref="InverseRequestSpecification"/> by
        /// decorating the supplied specification.
        /// </summary>
        /// <param name="specification">
        /// The <see cref="IRequestSpecification"/> to decorate.
        /// </param>
        public InverseRequestSpecification(IRequestSpecification specification)
        {
            if (specification == null)
            {
                throw new ArgumentNullException(nameof(specification));
            }

            this.spec = specification;
        }

        /// <summary>
        /// Gets the decorated specification.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is the <see cref="IRequestSpecification"/> that will be inverted.
        /// </para>
        /// </remarks>
        public IRequestSpecification Specification
        {
            get { return this.spec; }
        }

        /// <summary>
        /// Returns the opposite result as the decorated <see cref="IRequestSpecification"/>.
        /// </summary>
        /// <param name="request">The specimen request.</param>
        /// <returns>
        /// <see langword="true"/> if the decorated <see cref="IRequestSpecification"/> returns
        /// <see langword="false"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsSatisfiedBy(object request)
        {
            return !this.spec.IsSatisfiedBy(request);
        }
    }
}
