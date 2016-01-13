namespace Ploeh.AutoFixture.Kernel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    internal static class TypeExtensions
    {
        public static TAttribute GetRequestAttribute<TAttribute>(object request) where TAttribute : Attribute
        {
            var member = request as MemberInfo;
            if(member != null)
            {
                return member.GetCustomAttributes(typeof(TAttribute), inherit: true).Cast<TAttribute>().SingleOrDefault();
            }
            var parameter = request as ParameterInfo;
            if(parameter != null)
            {
                return parameter.GetCustomAttributes(typeof(TAttribute), inherit: true).Cast<TAttribute>().SingleOrDefault();
            }
            return null;
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
