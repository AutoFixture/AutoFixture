using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    public class MultipleToEnumerableRelay : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            var multipleRequest = request as MultipleRequest;
            if (multipleRequest == null)
                return new NoSpecimen(request);

            var innerRequest = multipleRequest.Request;
            var seededRequest = innerRequest as SeededRequest;
            if (seededRequest != null)
                innerRequest = seededRequest.Request;

            var itemType = innerRequest as Type;
            if (itemType == null)
                return new NoSpecimen(request);

            return context.Resolve(
                typeof(IEnumerable<>).MakeGenericType(itemType));
        }
    }
}
