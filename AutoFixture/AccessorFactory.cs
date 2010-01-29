using System;
using System.Reflection;

namespace Ploeh.AutoFixture
{
    internal abstract class AccessorFactory
    {
        protected AccessorFactory()
        {
        }

        internal abstract Accessor CreateAccessor();

        internal static AccessorFactory Create(MemberInfo member)
        {
            PropertyInfo pi = member as PropertyInfo;
            if (pi != null)
            {
                return new PropertyAccessorFactory(pi);
            }
            FieldInfo fi = member as FieldInfo;
            if (fi != null)
            {
                return new FieldAccessorFactory(fi);
            }
            UnexpectedInfo ui = member as UnexpectedInfo;
            if (ui != null)
            {
                return new UnEqualAccessorFactory(ui);
            }
            return new NullAccessorFactory(member);
        }

        internal abstract Accessor CreateAssignment(Func<Type, string, object> valueCreator);
    }
}
