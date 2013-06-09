namespace Ploeh.TestTypeFoundation
{
    public class ValueObjectWithoutEquals
    {
        private readonly int value;
        private readonly string currency;

        public ValueObjectWithoutEquals(int value, string currency)
        {
            this.value = value;
            this.currency = currency;
        }

        public int Value
        {
            get { return this.value; }
        }

        public string Currency
        {
            get { return this.currency; }
        } 
    }
}