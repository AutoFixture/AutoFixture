using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    public class OmitEnumerableParameterRequestRelay : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var pi = request as ParameterInfo;
            if (pi == null)
                return new NoSpecimen(request);

            if (!pi.ParameterType.IsGenericType)
                return new NoSpecimen(request);

            if (IsNotEnumerable(pi))
                return new NoSpecimen(request);

            var returnValue = context.Resolve(
                new SeededRequest(
                    pi.ParameterType,
                    pi.Name));

            if (returnValue is OmitSpecimen)
                return Array.CreateInstance(
                    pi.ParameterType.GetGenericArguments().Single(),
                    0);

            return returnValue;
        }

        private static bool IsNotEnumerable(ParameterInfo pi)
        {
            var openGenericType = pi.ParameterType.GetGenericTypeDefinition();
            return openGenericType != typeof(IEnumerable<>);
        }
    }
}
