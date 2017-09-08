namespace Ploeh.SemanticComparison.UnitTest.TestTypes
{
    public class TypeWithPublicFieldsAndProperties
    {
        public string Field;

        public long Number { get; set; }

        public decimal AutomaticProperty { get; set; }
    }
}
