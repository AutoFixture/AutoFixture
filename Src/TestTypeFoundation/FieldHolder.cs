using System.Reflection;

namespace TestTypeFoundation
{
    public class FieldHolder<T>
    {
        public T Field;

        public static FieldInfo GetField()
        {
            return typeof(FieldHolder<T>)
                .GetRuntimeField(nameof(Field));
        }
    }
}
