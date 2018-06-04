namespace TestTypeFoundation
{
    public struct MutableValueType
    {
        public MutableValueType(object property1, object property2, object property3)
            : this()
        {
            this.Property1 = property1;
            this.Property2 = property2;
            this.Property3 = property3;
        }

        public object Property1 { get; set; }

        public object Property2 { get; set; }

        public object Property3 { get; set; }
    }
}