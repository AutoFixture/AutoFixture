using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Unwraps a request for a <see cref="Seed"/> to a request for its
    /// <see cref="Seed.TargetType"/> while ignoring the <see cref="Seed.Value"/>.
    /// </summary>
    public class ValueIgnoringSeedUnwrapper : ISpecimenBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueIgnoringSeedUnwrapper"/> type.
        /// </summary>
        public ValueIgnoringSeedUnwrapper()
        {
        }

        #region ISpecimenBuilder Members

        /// <summary>
        /// Creates an anonymous value by unwrapping a seeded request and ignoring the seed.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="container">A container that can be used to create other specimens.</param>
        /// <returns>
        /// A specimen based on <paramref name="request"/> if possible; otherwise,
        /// <see langword="null"/>.
        /// </returns>
        /// <remarks>
        /// <para>
        /// If <paramref name="request"/> is a seeded request, the Create method unwraps the inner
        /// request and forwards it to <paramref name="container"/>. The seed value is ignored.
        /// </para>
        /// <para>
        /// The purpose of this class is to provide a fallback to handle seeded requests that no
        /// other <see cref="ISpecimenBuilder"/> instances have been able to handle. By ignoring
        /// the seed value, it handles those scenarios where the seed value and the inner request
        /// don't match and can't be combined, ensuring that at least some value is returned.
        /// </para>
        /// </remarks>
        public object Create(object request, ISpecimenContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            var seededRequest = request as Seed;
            if (seededRequest == null)
            {
                return null;
            }

            return container.Create(seededRequest.TargetType);
        }

        #endregion
    }
}
