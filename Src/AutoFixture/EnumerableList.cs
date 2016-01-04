﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture
{
    internal static class EnumerableList
    {
        internal static int IndexOf<T>(this IEnumerable<T> items, T item)
        {
            int i = 0;
            foreach (var s in items)
            {
                if (item.Equals(s))
                    return i;
                i++;
            }

            return -1;
        }

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
                throw new ArgumentOutOfRangeException(nameof(index), string.Format(CultureInfo.CurrentCulture, "The supplied index was less than zero ({0}). Only zero and positive index numbers are supported.", index));

            var a = items.ToArray();
            if (a.Length <= index)
                throw new ArgumentOutOfRangeException(nameof(index), string.Format(CultureInfo.CurrentCulture, "The supplied index ({0}) exceeds the addressable space of the current sequence. The length of the sequence is only {1}.", index, a.Length));

            return a.Take(index).Concat(new[] { item }).Concat(a.Skip(index + 1));
        }
    }
}
