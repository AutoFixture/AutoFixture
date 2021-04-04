using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace AutoFixture.Kernel
{
    internal static class EnumerableEnvy
    {
        public static IEnumerable<object> ConvertObjectType(this IEnumerable<object> enumerable, Type type)
        {
            return enumerable.Select(v => Convert.ChangeType(v, type, CultureInfo.CurrentCulture));
        }
    }
}