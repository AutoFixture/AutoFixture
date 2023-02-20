using System.Reflection;

namespace TestTypeFoundation
{
    public class PropertyHolder<T>
    {
        public T Property { get; set; }

        public void SetProperty(T value)
        {
            this.Property = value;
        }

        public static PropertyInfo GetProperty()
        {
            return typeof(PropertyHolder<T>)
                .GetRuntimeProperty(nameof(Property));
        }
    }
}
