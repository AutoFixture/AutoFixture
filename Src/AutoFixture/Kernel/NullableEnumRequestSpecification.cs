using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    public class NullableEnumRequestSpecification : IRequestSpecification
    {
        #region IRequestSpecification Members

        public bool IsSatisfiedBy(object request)
        {
            return (from t in request.Maybe().OfType<Type>()
                    where t.IsGenericType
                    let gtd = t.GetGenericTypeDefinition()
                    where typeof(Nullable<>).IsAssignableFrom(gtd)
                    let ga = t.GetGenericArguments()
                    where ga.Length == 1
                    select ga.Single().IsEnum).DefaultIfEmpty(false).Single();
        }

        #endregion
    }
}
