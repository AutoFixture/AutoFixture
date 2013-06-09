namespace Ploeh.TestTypeFoundation
{
    public class ValueObject
    {
        private readonly int value;
        private readonly string currency;

        public ValueObject(int value, string currency)
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

        public override bool Equals(object obj)
        {
            var objectToCompare = obj as ValueObject;
            return objectToCompare != null && this.Value == objectToCompare.Value && this.Currency == objectToCompare.Currency;
        }
    }
}