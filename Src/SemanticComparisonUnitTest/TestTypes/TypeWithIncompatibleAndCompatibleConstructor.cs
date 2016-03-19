using Ploeh.TestTypeFoundation;

namespace Ploeh.SemanticComparison.UnitTest.TestTypes
{
    public class TypeWithIncompatibleAndCompatibleConstructor
    {
        public TypeWithIncompatibleAndCompatibleConstructor(ConcreteType a)
            : this(new ConcreteType(), new CompositeType(a), new byte())
        {
        }

        public TypeWithIncompatibleAndCompatibleConstructor(
            ConcreteType a, 
            byte b)
            : this(new ConcreteType(), new CompositeType(a), b)
        {
        }

        public TypeWithIncompatibleAndCompatibleConstructor(
            ConcreteType a, 
            AbstractType b, 
            byte c)
        {
            this.Property1 = a;
            this.Property2 = b;
            this.Property3 = c;
        }

        public AbstractType Property1 { get; }

        public AbstractType Property2 { get; }

        public byte Property3 { get; }
    }
}
