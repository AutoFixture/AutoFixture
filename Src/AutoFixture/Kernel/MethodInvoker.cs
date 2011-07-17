using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    public class MethodInvoker : ISpecimenBuilder
    {
        private readonly IMethodQuery query;

        public MethodInvoker(IMethodQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            this.query = query;
        }

        public IMethodQuery Query
        {
            get { return this.query; }
        }

        #region ISpecimenBuilder Members

        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            foreach (var ci in this.GetConstructors(request))
            {
                var paramValues = (from pi in ci.Parameters
                                   select context.Resolve(pi)).ToList();

                if (paramValues.All(MethodInvoker.IsValueValid))
                {
                    return ci.Invoke(paramValues.ToArray());
                }
            }

            return new NoSpecimen(request);
        }

        #endregion

        private IEnumerable<IMethod> GetConstructors(object request)
        {
            var requestedType = request as Type;
            if (requestedType == null)
            {
                return Enumerable.Empty<IMethod>();
            }

            return this.query.SelectMethods(requestedType);
        }

        private static bool IsValueValid(object value)
        {
            return !(value is NoSpecimen);
        }
    }
}
