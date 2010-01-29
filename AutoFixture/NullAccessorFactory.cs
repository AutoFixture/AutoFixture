using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture
{
    internal class NullAccessorFactory : AccessorFactory
    {
        private readonly MemberInfo member;

        internal NullAccessorFactory(MemberInfo member)
        {
            this.member = member;
        }

        internal override Accessor CreateAccessor()
        {
            return new NullAccessor(this.member);
        }

        internal override Accessor CreateAssignment(Func<Type, string, object> valueCreator)
        {
            return new NullAccessor(this.member);
        }
    }
}
