namespace Ploeh.TestTypeFoundation
{
    public abstract class AbstractTypeWithConstructorWithMultipleParameters
    {
        private readonly int parameter1;
        private readonly int parameter2;

        protected AbstractTypeWithConstructorWithMultipleParameters(int parameter1, int parameter2)
        {
            this.parameter1 = parameter1;
            this.parameter2 = parameter2;
        }

        public int Parameter1
        {
            get { return parameter1; }
        }

        public int Parameter2
        {
            get { return parameter2; }
        }
    }
}