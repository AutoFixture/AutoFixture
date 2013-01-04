namespace Ploeh.TestTypeFoundation
{
    public abstract class AbstractTypeWithConstructorWithMultipleParameters
    {
        private readonly int property1;
        private readonly int property2;

        protected AbstractTypeWithConstructorWithMultipleParameters(int parameter1, int parameter2)
        {
            this.property1 = parameter1;
            this.property2 = parameter2;
        }

        public int Property1
        {
            get { return property1; }
        }

        public int Property2
        {
            get { return property2; }
        }
    }
}
