using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates new <see cref="DateTime"/> instances.
    /// </summary>
    public class DateTimeGenerator : ISpecimenBuilder
    {
        #region ISpecimenBuilder Members

        /// <summary>
        /// Creates a new <see cref="DateTime"/> instance.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">Not used.</param>
        /// <returns>
        /// A new <see cref="DateTime"/> instance, if <paramref name="request"/> is a request for a
        /// <see cref="DateTime"/>; otherwise, a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (request != typeof(DateTime))
            {
                return new NoSpecimen(request);
            }

            return DateTime.Now;
        }

        #endregion
    }
}
