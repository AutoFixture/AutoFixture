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
            if (attributes.Length == 1) return (TAttribute)attributes[0];

            throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                "Member '{0}' contains more than one attribute of type '{1}'", candidate, typeof(TAttribute)));
        }

        public static bool TryGetSingleGenericTypeArgument(
            this Type currentType, Type expectedGenericDefinition, out Type argument)
        {
            if (!expectedGenericDefinition.GetTypeInfo().IsGenericTypeDefinition)
                throw new ArgumentException("Must be a generic type definition", nameof(expectedGenericDefinition));

            var typeInfo = currentType.GetTypeInfo();
            if (typeInfo.IsGenericType && currentType.GetGenericTypeDefinition() == expectedGenericDefinition)
            {
                var typeArguments = typeInfo.GenericTypeArguments;
                if (typeArguments.Length == 1)
                {
                    argument = typeArguments[0];
                    return true;
                }
            }

            argument = null;
            return false;
        }

        public static bool IsNumberType(this Type type)
        {
            if (type.GetTypeInfo().IsEnum) return false;

            var typeCode = Type.GetTypeCode(type);
            switch (typeCode)
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return true;

                default:
                    return false;
            }
        }
    }
}
