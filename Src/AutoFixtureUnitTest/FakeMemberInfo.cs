using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoFixtureUnitTest
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
                    where MatchesInheritance(p, inherit)
                    select p.Attribute).ToArray();
        }

        public object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return (from p in this.providedAttributes
                    where p.Attribute.GetType() == attributeType && MatchesInheritance(p, inherit)
                    select p.Attribute).ToArray();
        }

        public bool IsDefined(Type attributeType, bool inherit)
        {
            return (from p in this.providedAttributes
                    where p.Attribute.GetType() == attributeType && MatchesInheritance(p, inherit)
                    select p.Attribute).Any();
        }

        private static bool MatchesInheritance(ProvidedAttribute attribute, bool inherit)
        {
            if (!inherit && attribute.Inherited)
            {
                return false;
            }

            return true;
        }
    }
}