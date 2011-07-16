using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.TestTypeFoundation
{
    public class TypeWithFactoryProperty
    {
        private TypeWithFactoryProperty()
        {
        }

        public static TypeWithFactoryProperty Factory
        {
            get
            {
                return new TypeWithFactoryProperty();
            }
        }
    }
}
