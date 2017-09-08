using System;
using System.Collections.Generic;
using System.Linq;

namespace Ploeh.TestTypeFoundation
{
    public class CompositeType : AbstractType
    {
        public CompositeType(IEnumerable<AbstractType> types)
            : this(types.ToArray())
        {
        }

        public CompositeType(params AbstractType[] types)
        {
            if (types == null)
            {
                throw new ArgumentNullException(nameof(types));
            }

            this.Types = types;
        }

        public IEnumerable<AbstractType> Types { get; }
    }
}