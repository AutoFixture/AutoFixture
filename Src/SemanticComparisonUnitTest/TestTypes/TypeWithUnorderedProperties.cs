using Ploeh.TestTypeFoundation;

namespace Ploeh.SemanticComparison.UnitTest.TestTypes
{
    public class TypeWithUnorderedProperties
    {
        public TypeWithUnorderedProperties(ConcreteType a, AbstractType b, byte c)
        {
            this.Property1 = a;
            this.Property2 = b;
            this.Property3 = c;
        }

        public byte Property3 { get; }

        public AbstractType Property1 { get; }

        public AbstractType Property2 { get; }
    }
}
