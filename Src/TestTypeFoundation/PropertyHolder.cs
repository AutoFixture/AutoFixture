namespace TestTypeFoundation
{
    public class PropertyHolder<T>
    {
        public T Property { get; set; }

        public void SetProperty(T value)
        {
            this.Property = value;
        }
    }
}
