using System;
using System.Reflection;

namespace Ploeh.AutoFixture
{
    internal class PropertyAccessorFactory : AccessorFactory
    {
        private readonly PropertyInfo pi;

        internal PropertyAccessorFactory(PropertyInfo property)
        {
            this.pi = property;
        }

        internal override Accessor CreateAccessor()
        {
            return new PropertyAccessor(this.pi, null);
        }

        internal override Accessor CreateAssignment(Func<Type, string, object> valueCreator)
        {
            if (this.CreateAccessor().CanWrite)
            {
                return new PropertyAccessor(this.pi, valueCreator(this.pi.PropertyType, this.pi.Name));
            }
            return new NullAccessor(this.pi);
        }
    }
}
