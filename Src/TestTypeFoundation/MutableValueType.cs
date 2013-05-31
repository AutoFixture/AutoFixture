namespace Ploeh.TestTypeFoundation
{
    public struct MutableValueType
    {
        public MutableValueType(object property1, object property2, object property3): this()
        {
            Property1 = property1;
            Property2 = property2;
            Property3 = property3;
        }

        public object Property1 { get; set; }

        public object Property2 { get; set; }

        public object Property3 { get; set; }
    }
}