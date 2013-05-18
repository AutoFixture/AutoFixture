namespace Ploeh.SemanticComparison.UnitTest
{
    public class TypeWithPublicFieldsAndProperties
    {
        public string Field;

        private long number;

        public long Number
        {
            get { return this.number; }
            set { this.number = value; }
        }

        public decimal AutomaticProperty { get; set; }
    }
}
