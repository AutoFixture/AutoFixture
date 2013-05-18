using Ploeh.TestTypeFoundation;

namespace Ploeh.SemanticComparison.UnitTest.TestTypes
{
    public class TypeWithUnorderedProperties
    {
        private readonly AbstractType value1;
        private readonly AbstractType value2;
        private readonly byte value3;

        public TypeWithUnorderedProperties(ConcreteType a, AbstractType b, byte c)
        {
            this.value1 = a;
            this.value2 = b;
            this.value3 = c;
        }

        public byte Property3
        {
            get { return this.value3; }
        }

        public AbstractType Property1
        {
            get { return this.value1; }
        }

        public AbstractType Property2
        {
            get { return this.value2; }
        }
    }
}
