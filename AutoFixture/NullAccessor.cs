using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture
{
    internal class NullAccessor : Accessor
    {
        internal NullAccessor(MemberInfo member)
            : base(member, null)
        {
        }

        internal override bool CanRead
        {
            get { return false; }
        }

        internal override bool CanWrite
        {
            get { return false; }
        }

        internal override void AssignOn(object obj)
        {
        }

        internal override object ReadFrom(object obj)
        {
            return null;
        }
    }
}
