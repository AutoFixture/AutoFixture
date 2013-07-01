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
            if (context == null)
                throw new ArgumentNullException("context");
            
            var multipleRequest = request as MultipleRequest;
            if (multipleRequest == null)
                return new NoSpecimen(request);

            var innerRequest = GetInnerRequest(multipleRequest);

            var itemType = innerRequest as Type;
            if (itemType == null)
                return new NoSpecimen(request);

            return context.Resolve(
                typeof(IEnumerable<>).MakeGenericType(itemType));
        }

        private static object GetInnerRequest(MultipleRequest multipleRequest)
        {
            var seededRequest = multipleRequest.Request as SeededRequest;
            if (seededRequest == null)
                return multipleRequest.Request;

            return seededRequest.Request;
        }
    }
}
