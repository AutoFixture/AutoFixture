namespace Ploeh.AutoFixture.Kernel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    internal static class TypeExtensions
    {
        public static TypeCode GetTypeCode(this Type type)
        {
            if(!type.IsValueType)
            {
                return TypeCode.Object;
            }
            return GetTypeCode(Activator.CreateInstance(type));
        }

        public static TypeCode GetTypeCode(object request)
        {
            var convertible = request as IConvertible;
            if(convertible == null)
            {
                return TypeCode.Object;
            }
            return convertible.GetTypeCode();
        }
    }
}