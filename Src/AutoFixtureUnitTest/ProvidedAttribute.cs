using System;

namespace Ploeh.AutoFixtureUnitTest
{
    internal class ProvidedAttribute
    {
        public ProvidedAttribute(Attribute attribute, bool inherited)
        {
            this.Attribute = attribute;
            this.Inherited = inherited;
        }

        public Attribute Attribute { get; }

        public bool Inherited { get; }
    }
}
