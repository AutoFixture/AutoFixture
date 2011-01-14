using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    public class EnumerableRelay : ISpecimenBuilder
    {
        #region ISpecimenBuilder Members

        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var type = request as Type;
            if (type == null)
            {
                return new NoSpecimen(request);
            }

            var typeArguments = type.GetGenericArguments();
            if (typeArguments.Length != 1)
            {
                return new NoSpecimen(request);
            }
            var typeArgument = typeArguments.Single();

            if (!typeof(IEnumerable<>).MakeGenericType(typeArgument).IsAssignableFrom(type))
            {
                return new NoSpecimen(request);
            }

            var enumerable = context.Resolve(new MultipleRequest(typeArgument)) as IEnumerable<object>;
            if (enumerable == null)
            {
                return new NoSpecimen(request);
            }
            return typeof(ConvertedEnumerable<>).MakeGenericType(typeArgument).GetConstructor(new[] { typeof(IEnumerable<object>) }).Invoke(new[] { enumerable });
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
