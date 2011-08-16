using System;
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
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <exception cref="ArgumentNullException"><paramref name="context"/> is null.</exception>
        /// <returns>
        /// A new <see cref="DateTime"/> instance, if <paramref name="request"/> is a request for a
        /// <see cref="DateTime"/>; otherwise, a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (request != typeof(DateTime))
            {
                return new NoSpecimen(request);
            }

            return DateTime.Now.AddDays(context.CreateAnonymous<int>());
        }

        #endregion
    }
}