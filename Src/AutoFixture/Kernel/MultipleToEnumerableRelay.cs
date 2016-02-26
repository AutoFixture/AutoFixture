using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Relays a <see cref="MultipleRequest" /> to a request for
    /// <see cref="IEnumerable{T}" />.
    /// </summary>
    /// <seealso cref="MultipleToEnumerableRelay.Create(object, ISpecimenContext)" />
    /// <seealso cref="MapCreateManyToEnumerable" />
    public class MultipleToEnumerableRelay : ISpecimenBuilder
    {
        /// <summary>
        /// Creates a new specimen based on a request.
        /// </summary>
        /// <param name="request">
        /// The request that describes what to create.
        /// </param>
        /// <param name="context">
        /// A context that can be used to create other specimens.
        /// </param>
        /// <returns>
        /// The requested specimen if possible; otherwise a
        /// <see cref="NoSpecimen" /> instance.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// context is null
        /// </exception>
        /// <remarks>
        /// <para>
        /// The <paramref name="request" /> can be any object, but will often
        /// be a <see cref="Type" /> or other
        /// <see cref="System.Reflection.MemberInfo" /> instances. This 
        /// particular implementation only handles
        /// <see cref="MultipleRequest" /> instances.
        /// <see cref="MultipleRequest.Request" /> must either be a Type
        /// instance, or a <see cref="SeededRequest" /> with a Type as its
        /// <see cref="SeededRequest.Request" />. If the request doesn't
        /// satisfy these conditions, a <see cref="NoSpecimen" /> instance is
        /// returned.
        /// </para>
        /// </remarks>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            
            var multipleRequest = request as MultipleRequest;
            if (multipleRequest == null)
#pragma warning disable 618
                return new NoSpecimen(request);
#pragma warning restore 618

            var innerRequest = GetInnerRequest(multipleRequest);

            var itemType = innerRequest as Type;
            if (itemType == null)
#pragma warning disable 618
                return new NoSpecimen(request);
#pragma warning restore 618

            return context.Resolve(
                typeof(IEnumerable<>).MakeGenericType(itemType));
        }

        private static object GetInnerRequest(MultipleRequest multipleRequest)
        {
            var seededRequest = multipleRequest.Request as SeededRequest;
            if (seededRequest == null)
                return multipleRequest.Request;

            return seededRequest.Request;
        }
    }
}
