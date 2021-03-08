using System;
using System.Linq;

namespace AutoFixture.Idioms
{
    internal static class TypeExtensions
    {
        public static bool ImplementsGenericInterfaceDefinition(this Type type, Type interfaceType)
            => type
                .GetInterfaces()
                .Any(i => i
                    .GetGenericTypeDefinition()
                    .IsAssignableFrom(interfaceType));

        public static bool ImplementsGenericInterface(this Type type, Type interfaceType, params Type[] argumentTypes)
            => type
                .GetInterfaces()
                .Any(i => i
                    .IsAssignableFrom(interfaceType.MakeGenericType(argumentTypes)));

        public static object GetDefaultValue(this Type type)
            => type.IsValueType
                ? Activator.CreateInstance(type)
                : null;
    }
}
