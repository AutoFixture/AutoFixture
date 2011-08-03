using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    public class TypeGenerator : ISpecimenBuilder
    {
        #region ISpecimenBuilder Members

        public object Create(object request, ISpecimenContext context)
        {
            if (typeof(Type).Equals(request))
            {
                return typeof(object);
            }

            return new NoSpecimen(request);
        }

        #endregion
    }
}
