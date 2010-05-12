using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture
{
    internal class FieldAccessorFactory : AccessorFactory
    {
        private readonly FieldInfo fi;

        internal FieldAccessorFactory(FieldInfo field)
        {
            this.fi = field;
        }

        internal override Accessor CreateAccessor()
        {
            return new FieldAccessor(this.fi, null);
        }

        internal override Accessor CreateAssignment(Func<Type, string, object> valueCreator)
        {
            if (this.CreateAccessor().CanWrite)
            {
                return new FieldAccessor(this.fi, valueCreator(this.fi.FieldType, this.fi.Name));
            }
            return new NullAccessor(this.fi);
        }
    }
}
