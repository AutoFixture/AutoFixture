using System;

namespace Ploeh.TestTypeFoundation
{
    public class ValueObjectWithIEquatableImplemented : IEquatable<ValueObjectWithIEquatableImplemented>
    {
        private readonly int value;
        private readonly string currency;

        public ValueObjectWithIEquatableImplemented(int value, string currency)
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

        public bool Equals(ValueObjectWithIEquatableImplemented objectToCompare)
        {
            return this.Value == objectToCompare.Value && this.Currency == objectToCompare.Currency;
        }
    }
}