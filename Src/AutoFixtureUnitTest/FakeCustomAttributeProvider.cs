using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ploeh.AutoFixtureUnitTest
{
    internal class FakeCustomAttributeProvider : ICustomAttributeProvider
    {
        private readonly IEnumerable<ProvidedAttribute> providedAttributes;

        public FakeCustomAttributeProvider(params ProvidedAttribute[] providedAttributes)
        {
            this.providedAttributes = providedAttributes;
        }

        public object[] GetCustomAttributes(bool inherit)
        {
            return (from p in this.providedAttributes
                    where IsMatching(p, inherit)
                    select p.Attribute).ToArray();
        }

        public object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return (from p in this.providedAttributes
                    where IsMatching(p, attributeType, inherit)
                    select p.Attribute).ToArray();
        }

        public bool IsDefined(Type attributeType, bool inherit)
        {
            return (from p in this.providedAttributes
                    where IsMatching(p, attributeType, inherit)
                    select p.Attribute).Any();
        }

        private bool IsMatching(ProvidedAttribute p, Type attributeType, bool inherit)
        {
            return attributeType.IsAssignableFrom(p.Attribute.GetType()) && IsMatching(p, inherit);
        }

        private bool IsMatching(ProvidedAttribute p, bool inherit)
        {
            return !p.Inherited || p.Inherited == inherit;
        }
    }
}