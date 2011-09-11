using System;

namespace Ploeh.AutoFixtureUnitTest
{
    internal class ProvidedAttribute
    {
        private readonly Attribute attribute;
        private readonly bool inherited;

        public ProvidedAttribute(Attribute attribute, bool inherited)
        {
            this.attribute = attribute;
            this.inherited = inherited;
        }

        public Attribute Attribute
        {
            get
            {
                return this.attribute;
            }
        }

        public bool Inherited
        {
            get
            {
                return this.inherited;
            }
        }
    }
}
