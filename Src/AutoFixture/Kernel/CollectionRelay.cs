using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Relays a request for an <see cref="ICollection{T}" /> to a request for a
    /// <see cref="List{T}"/> and retuns the result.
    /// </summary>
    public class CollectionRelay : ISpecimenBuilder
    {
        #region ISpecimenBuilder Members

        /// <summary>
        /// Creates a new specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// A populated <see cref="ICollection{T}"/> of the appropriate item type if possible;
        /// otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        /// <remarks>
        /// <para>
        /// If <paramref name="request"/> is a request for an <see cref="ICollection{T}"/> and
        /// <paramref name="context"/> can satisfy a request for a populated specimen of that type,
        /// this value will be returned. If not, the return value is a <see cref="NoSpecimen"/>
        /// instance.
        /// </para>
        /// </remarks>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return (from t in request.Maybe().OfType<Type>()
                    let typeArguments = t.GetGenericArguments()
                    where typeArguments.Length == 1
                    && typeof(ICollection<>) == t.GetGenericTypeDefinition()
                    select context.Resolve(typeof(List<>).MakeGenericType(typeArguments)))
                    .DefaultIfEmpty(new NoSpecimen(request))
                    .Single();
        }

        #endregion
    }
}
