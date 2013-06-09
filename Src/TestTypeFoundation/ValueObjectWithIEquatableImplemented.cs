using System;

namespace Ploeh.TestTypeFoundation
{
    public class ValueObjectWithIEquatableImplemented : IEquatable<ValueObjectWithIEquatableImplemented>
    {
        private readonly int value;
        private readonly string currency;
        private readonly ValueObject valueObject;

        public ValueObjectWithIEquatableImplemented(int value, string currency, ValueObject valueObject)
        {
            this.value = value;
            this.currency = currency;
            this.valueObject = valueObject;
        }

        public int Value
        {
            get { return this.value; }
        }

        public string Currency
        {
            get { return this.currency; }
        }

        public ValueObject ValueObject
        {
            get { return this.valueObject; }
        }

        public bool Equals(ValueObjectWithIEquatableImplemented objectToCompare)
        {
            return this.Value == objectToCompare.Value && this.Currency == objectToCompare.Currency && this.ValueObject.Equals(objectToCompare.ValueObject);
        }
    }
}