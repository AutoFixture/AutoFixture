using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Ploeh.AutoFixture.Kernel
{
    public class CollectionSpecification : IRequestSpecification
    {
        #region IRequestSpecification Members

        public bool IsSatisfiedBy(object request)
        {
            var type = request as Type;
            if (type == null)
            {
                return false;
            }

            return type.IsGenericType
                && typeof(Collection<>) == type.GetGenericTypeDefinition();
        }

        #endregion
    }
}
