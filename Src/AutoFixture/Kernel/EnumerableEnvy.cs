using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace AutoFixture.Kernel
{
    internal static class EnumerableEnvy
    {
        public static IEnumerable<object> ConvertObjectType(this IEnumerable<object> enumerable, Type type)
        {
            return enumerable.Select(v => v == null || type.GetTypeInfo().IsAssignableFrom(v.GetType().GetTypeInfo()) ? v : Convert.ChangeType(v, type, CultureInfo.CurrentCulture));
        }
    }
}
