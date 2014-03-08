using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    public class OmitArrayParameterRequestRelay : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            var pi = request as ParameterInfo;
            if (pi == null)
                return new NoSpecimen(request);

            return context.Resolve(
                new SeededRequest(
                    pi.ParameterType,
                    pi.Name));
        }
    }
}
