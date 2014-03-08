using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    public class OmitEnumerableParameterRequestRelay : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            throw new NotImplementedException();
        }
    }
}
