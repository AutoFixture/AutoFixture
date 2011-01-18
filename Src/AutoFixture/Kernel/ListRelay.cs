using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    public class ListRelay : ISpecimenBuilder
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

            if (typeof(IList<>) != type.GetGenericTypeDefinition())
            {
                return new NoSpecimen(request);
            }

            return context.Resolve(typeof(List<>).MakeGenericType(typeArgument));
        }

        #endregion
    }
}
