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

        internal static IEnumerable<T> SetItem<T>(this IEnumerable<T> items, int index, T item)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", string.Format("The supplied index was less than zero ({0}). Only zero and positive index numbers are supported.", index));

            var a = items.ToArray();
            if (a.Length <= index)
                throw new ArgumentOutOfRangeException("index", string.Format("The supplied index ({0}) exceeds the addressable space of the current sequence. The length of the sequence is only {1}.", index, a.Length));

            return a.Take(index).Concat(new[] { item }).Concat(a.Skip(index + 1));
        }
    }
}
