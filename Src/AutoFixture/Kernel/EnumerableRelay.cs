using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Relays a request for <see cref="IEnumerable{T}" /> to a <see cref="MultipleRequest"/> and
    /// converts the result to a sequence of the requested type.
    /// </summary>
    public class EnumerableRelay : ISpecimenBuilder
    {
        #region ISpecimenBuilder Members

        /// <summary>
        /// Creates a new sequence of items based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// A sequence of the requested type if possible; otherwise a <see cref="NoSpecimen"/>
        /// instance.
        /// </returns>
        /// <remarks>
        /// <para>
        /// If <paramref name="request"/> is a request for <see cref="IEnumerable{T}" /> and
        /// <paramref name="context"/> can satisfy a <see cref="MultipleRequest"/> for the item
        /// type, the return value is a populated sequence of the requested type. If not, the
        /// return value is a <see cref="NoSpecimen"/> instance.
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
                    && typeof(IEnumerable<>) == t.GetGenericTypeDefinition()
                    let enumerable = context.Resolve(new MultipleRequest(typeArguments.Single())) as IEnumerable<object>
                    where enumerable != null
                    select typeof(ConvertedEnumerable<>).MakeGenericType(typeArguments).GetConstructor(new[] { typeof(IEnumerable<object>) }).Invoke(new[] { enumerable }))
                    .DefaultIfEmpty(new NoSpecimen(request))
                    .Single();
        }

        #endregion

        private class ConvertedEnumerable<T> : IEnumerable<T>
        {
            private readonly IEnumerable<object> enumerable;

            public ConvertedEnumerable(IEnumerable<object> enumerable)
            {
                if (enumerable == null)
                {
                    throw new ArgumentNullException("enumerable");
                }

                this.enumerable = enumerable;
            }

            #region IEnumerable<T> Members

            public IEnumerator<T> GetEnumerator()
            {
                foreach (var item in this.enumerable)
                {
                    yield return (T)item;
                }
            }

            #endregion

            #region IEnumerable Members

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            #endregion
        }
    }
}
