using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates new <see cref="Guid"/> instances.
    /// </summary>
    public class GuidGenerator : ISpecimenBuilder
    {
        /// <summary>
        /// Creates a new <see cref="Guid"/> instance.
        /// </summary>
        /// <returns>A new <see cref="Guid"/> instance.</returns>
        public static Guid CreateAnonymous()
        {
            return Guid.NewGuid();
        }

        /// <summary>
        /// Creates a new <see cref="Guid"/> instance.
        /// </summary>
        /// <param name="seed">Ignored.</param>
        /// <returns>A new <see cref="Guid"/> instance.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static object CreateAnonymous(object seed)
        {
            return GuidGenerator.CreateAnonymous();
        }

        #region ISpecimenBuilder Members

        /// <summary>
        /// Creates a new <see cref="Guid"/> instance.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="container">Not used.</param>
        /// <returns>
        /// A new <see cref="Guid"/> instance, if <paramref name="request"/> is a request for a
        /// <see cref="Guid"/>; otherwise, a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContainer container)
        {
            if (request != typeof(Guid))
            {
                return new NoSpecimen(request);
            }

            return GuidGenerator.CreateAnonymous();
        }

        #endregion
    }
}
