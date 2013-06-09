namespace Ploeh.TestTypeFoundation
{
    public class MutableValueObject
    {
        private int value;
        private string currency;

        public MutableValueObject()
        {}

        public MutableValueObject(string currency)
        {
            this.currency = currency;
        }

        public MutableValueObject(int value)
        {
            this.value = value;
        }

        public MutableValueObject(int value, string currency)
        {
            this.value = value;
            this.currency = currency;
        }

        public int Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public string Currency
        {
            get { return this.currency; }
            set { this.currency = value; }
        }

        public override bool Equals(object obj)
        {
            var objectToCompare = obj as MutableValueObject;
            return objectToCompare != null && this.Value == objectToCompare.Value && this.Currency == objectToCompare.Currency;
        } 
    }
}