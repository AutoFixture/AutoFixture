using System.Linq;
using System.Reflection;

namespace TestTypeFoundation
{
    public class UnguardedConstructorHost<T>
    {
        public UnguardedConstructorHost(T item)
        {
            this.Item = item;
        }

        public T Item { get; }

        private static ConstructorInfo GetConstructor()
        {
            var typeInfo = typeof(UnguardedConstructorHost<T>)
                .GetTypeInfo();

            return typeInfo.DeclaredConstructors.Single();
        }

        public static ParameterInfo GetParameter()
        {
            return GetConstructor().GetParameters().Single();
        }
    }
}