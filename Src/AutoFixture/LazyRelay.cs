using Ploeh.AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture
{
    public class LazyRelay : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            if (request == null)
                return new NoSpecimen();

            var t = request as Type;
            if (t == null || !t.IsGenericType)
                return new NoSpecimen();

            throw new NotImplementedException();
        }
    }
}
