using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture
{
    internal static class EnumerableList
    {
        internal static IEnumerable<T> Insert<T>(this IEnumerable<T> items, int index, T item)
        {
            return items.Take(index).Concat(new[] { item }).Concat(items.Skip(index));
        }

        internal static IEnumerable<T> RemoveAt<T>(this IEnumerable<T> items, int index)
        {
            return items.Take(index).Concat(items.Skip(index + 1));
        }
    }
}
