using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ploeh.AutoFixtureUnitTest
{
    internal class FakeMemberInfo : ICustomAttributeProvider
    {
        private readonly IEnumerable<ProvidedAttribute> providedAttributes;

        public FakeMemberInfo(params ProvidedAttribute[] providedAttributes)
        {
            this.providedAttributes = providedAttributes;
        }

        public object[] GetCustomAttributes(bool inherit)
        {
            return (from p in this.providedAttributes
                    where p.Inherited == inherit
                    select p.Attribute).ToArray();
        }

        public object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return (from p in this.providedAttributes
                    where p.Attribute.GetType() == attributeType && p.Inherited == inherit
                    select p.Attribute).ToArray();
        }

        public bool IsDefined(Type attributeType, bool inherit)
        {
            return (from p in this.providedAttributes
                    where p.Attribute.GetType() == attributeType && p.Inherited == inherit
                    select p.Attribute).Any();
        }
    }
}