using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture
{
    internal class UnexpectedInfo : MemberInfo
    {
        public override Type DeclaringType
        {
            get { return typeof(object); }
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return new object[0];
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            return new object[0];
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            return false;
        }

        public override MemberTypes MemberType
        {
            get { return MemberTypes.Custom; }
        }

        public override string Name
        {
            get { return "Unexpected"; }
        }

        public override Type ReflectedType
        {
            get { return typeof(object); }
        }
    }
}
