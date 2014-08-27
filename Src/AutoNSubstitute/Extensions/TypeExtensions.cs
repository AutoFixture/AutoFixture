using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.AutoNSubstitute.Extensions
{
    internal static class TypeExtensions
    {
        public static object GetDefault(this Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
        public static Type GetSubstituteType(this Type type)
        {
            if (type.BaseType == typeof(object))
                return type.GetInterfaces().First();

            return type.BaseType;
        }

    }
}
