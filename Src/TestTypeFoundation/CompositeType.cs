using System;
using System.Collections.Generic;
using System.Linq;

namespace Ploeh.TestTypeFoundation
{
    public class CompositeType : AbstractType
    {
        private readonly IEnumerable<AbstractType> types;

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

            this.types = types;
        }

        public IEnumerable<AbstractType> Types
        {
            get { return this.types; }
        }
    }
}