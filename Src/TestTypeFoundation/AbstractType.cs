namespace TestTypeFoundation
{
    public abstract class AbstractType
    {
        protected AbstractType()
        {
        }

        public object Property1 { get; set; }

        public object Property2 { get; set; }

        public object Property3 { get; set; }

        public virtual object Property4 { get; set; }

        public object Field1;
    }
}
