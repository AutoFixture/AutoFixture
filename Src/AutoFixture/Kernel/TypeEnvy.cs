using System;
using System.Globalization;
using System.Reflection;

namespace AutoFixture.Kernel
{
    internal static class TypeEnvy
    {
        public static TAttribute GetAttribute<TAttribute>(object candidate)
            where TAttribute : Attribute
        {
            // This is performance critical code and Linq isn't being used intentionally.
            var attributeProvider = candidate as ICustomAttributeProvider;
            if (attributeProvider == null) return null;

            var attributes = attributeProvider.GetCustomAttributes(typeof(TAttribute), true);

            if (attributes.Length == 0) return null;
            if (attributes.Length == 1) return (TAttribute) attributes[0];

            throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                "Member '{0}' contains more than one attribute of type '{1}'", candidate, typeof(TAttribute)));
        }

        public static Assembly Assembly(this Type type)
        {
            return type.GetTypeInfo().Assembly;
        }

        public static Type BaseType(this Type type)
        {
            return type.GetTypeInfo().BaseType;
        }

        public static bool IsAbstract(this Type type)
        {
            return type.GetTypeInfo().IsAbstract;
        }

        public static bool IsClass(this Type type)
        {
            return type.GetTypeInfo().IsClass;
        }

        public static bool IsEnum(this Type type)
        {
            return type.GetTypeInfo().IsEnum;
        }

        public static bool IsGenericType(this Type type)
        {
            return type.GetTypeInfo().IsGenericType;
        }

        public static bool IsGenericTypeDefinition(this Type type)
        {
            return type.GetTypeInfo().IsGenericTypeDefinition;
        }

        public static bool IsInterface(this Type type)
        {
            return type.GetTypeInfo().IsInterface;
        }

        public static bool IsPrimitive(this Type type)
        {
            return type.GetTypeInfo().IsPrimitive;
        }

        public static bool IsValueType(this Type type)
        {
            return type.GetTypeInfo().IsValueType;
        }
    }
}
