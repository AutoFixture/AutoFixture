using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// A <see cref="IRequestSpecification"/> that is always <see langword="false"/>.
    /// </summary>
    public class FalseRequestSpecification : IRequestSpecification
    {
        #region IRequestSpecification Members

        /// <summary>
        /// Evaluates a request for a specimen.
        /// </summary>
        /// <param name="request">The specimen request.</param>
        /// <returns>
        /// <see langword="false"/>.
        /// </returns>
        public bool IsSatisfiedBy(object request)
        {
            return false;
        }

        #endregion
    }
}
