using System;
using System.Collections;
using System.Linq;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Relays a request for an array to a <see cref="MultipleRequest"/> and converts the result
    /// to the desired array type.
    /// </summary>
    public class ArrayRelay : ISpecimenBuilder
    {
        /// <summary>
        /// Creates a new array based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// An array of the requested type if possible; otherwise a <see cref="NoSpecimen"/>
        /// instance.
        /// </returns>
        /// <remarks>
        /// <para>
        /// If <paramref name="request"/> is a request for an array and <paramref name="context"/>
        /// can satisfy a <see cref="MultipleRequest"/> for the element type, the return value is a
        /// populated array of the requested type. If not, the return value is a
        /// <see cref="NoSpecimen"/> instance.
        /// </para>
        /// </remarks>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return (from t in request.Maybe().OfType<Type>()
                    where t.IsArray
                    let elementType = t.GetElementType()
                    let e = context.Resolve(new MultipleRequest(elementType)) as IEnumerable
                    where e != null
                    select ArrayRelay.ToArray(e, elementType))
                    .DefaultIfEmpty(new NoSpecimen(request))
                    .Single();
        }

        private static object ToArray(IEnumerable e, Type elementType)
        {
            var al = new ArrayList();
            foreach (var element in e)
            {
                al.Add(element);
            }
            return al.ToArray(elementType);
        }
    }
}
