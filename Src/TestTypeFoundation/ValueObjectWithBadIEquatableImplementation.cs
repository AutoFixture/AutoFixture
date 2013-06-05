using System;

namespace Ploeh.TestTypeFoundation
{
    public class ValueObjectWithBadIEquatableImplementation : IEquatable<ValueObjectWithBadIEquatableImplementation>
    {
        private readonly int value;
        private readonly string currency;

        public ValueObjectWithBadIEquatableImplementation(int value, string currency)
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

        public bool Equals(ValueObjectWithBadIEquatableImplementation objectToCompare)
        {
            return this.Value == objectToCompare.Value;
        } 
    }
}