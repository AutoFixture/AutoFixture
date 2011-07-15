using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Ploeh.TestTypeFoundation
{
    public class TypeWithFactoryProperty
    {
        private static TypeWithFactoryProperty value;

        private TypeWithFactoryProperty()
        {
        }

        public static TypeWithFactoryProperty Instance
        {
            get
            {
                if (value != null)
                {
                    return value;
                }

                Interlocked.CompareExchange(ref value, new TypeWithFactoryProperty(), null);

                return value;
            }
        }
    }
}
