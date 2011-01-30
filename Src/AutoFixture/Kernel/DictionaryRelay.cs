using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    public class DictionaryRelay : ISpecimenBuilder
    {
        #region ISpecimenBuilder Members

        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

#warning Pretty much same implementation for CollectionRelay, ListRelay, EnumerableRelay and DictionaryRelay. Should be refactored, but it seems more and more likely that the correct solution is to use a Maybe monad, so we'll wait for that before refactoring.
            var type = request as Type;
            if (type == null)
            {
                return new NoSpecimen(request);
            }

            var typeArguments = type.GetGenericArguments();
            if (typeArguments.Length != 2)
            {
                return new NoSpecimen(request);
            }

            if (typeof(IDictionary<,>) != type.GetGenericTypeDefinition())
            {
                return new NoSpecimen(request);
            }

            return context.Resolve(typeof(Dictionary<,>).MakeGenericType(typeArguments));
        }

        #endregion
    }
}
