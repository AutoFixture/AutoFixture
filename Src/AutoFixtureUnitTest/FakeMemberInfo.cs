using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ploeh.AutoFixtureUnitTest
{
    internal class FakeMemberInfo : MemberInfo
    {
        private readonly IEnumerable<ProvidedAttribute> providedAttributes;

        public FakeMemberInfo(params ProvidedAttribute[] providedAttributes)
        {
            this.providedAttributes = providedAttributes;
        }

        public override Type DeclaringType
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override MemberTypes MemberType
        {
            get
            {
                return MemberTypes.All;
            }
        }

        public override string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override Type ReflectedType
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            return (from p in this.providedAttributes
                    where p.Inherited == inherit
                    select p.Attribute).ToArray();
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return (from p in this.providedAttributes
                    where p.Attribute.GetType() == attributeType && p.Inherited == inherit
                    select p.Attribute).ToArray();
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            return (from p in this.providedAttributes
                    where p.Attribute.GetType() == attributeType && p.Inherited == inherit
                    select p.Attribute).Any();
        }
    }
}