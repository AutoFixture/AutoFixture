using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// An <see cref="IRequestSpecification"/> that always returns <see langword="true"/>.
    /// </summary>
    public class InclusiveRequestSpecification : IRequestSpecification
    {
        #region IRequestSpecification Members

        /// <summary>
        /// Always returns true.
        /// </summary>
        /// <param name="request">The specimen request.</param>
        /// <returns><see langword="true"/>.</returns>
        public bool IsSatisfiedBy(object request)
        {
            return true;
        }

        #endregion
    }
}
