using System;
using System.Collections.Generic;

namespace AutoFixture.NUnit3
{
    internal static class EnumerableExtensions
    {
        public static IEnumerable<T> Tap<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action?.Invoke(item);
                yield return item;
            }
        }
    }
}