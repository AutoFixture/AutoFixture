using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoRhinoMock
{
    public class DefaultGenericListBuilder : ISpecimenBuilder
    {
        #region ISpecimenBuilder Members

        public object Create(object request, ISpecimenContext context)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var t = request as Type;
            if (t == null || !t.IsGenericType)
            {
                return new NoSpecimen(request);
            }

            if (t.GetGenericArguments().Count() == 1)
            {
                var listType = typeof(List<>).MakeGenericType(t.GetGenericArguments());
                if (t.IsAssignableFrom(listType))
                {
                    return Activator.CreateInstance(listType, false);
                }
            }

            return new NoSpecimen(request);
       }

        #endregion
    }
}
