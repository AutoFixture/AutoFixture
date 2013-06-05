namespace Ploeh.TestTypeFoundation
{
    public class ValueObjectWithoutConstructorAndBadEquals
    {
        public int Value { get; set; }

        public string Currency { get; set; }

        public override bool Equals(object obj)
        {
            var objectToCompare = obj as ValueObject;
            return objectToCompare != null && this.Currency == objectToCompare.Currency;
        } 
    }
}