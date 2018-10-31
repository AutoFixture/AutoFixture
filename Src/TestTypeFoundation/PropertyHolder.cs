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

    public class PrivatePropertyHolder<T>
    {
        public T Property { get; private set; }

        public void SetProperty(T value)
        {
            this.Property = value;
        }
    }
}