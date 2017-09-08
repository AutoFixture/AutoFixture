using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    internal static class EnumerableComparison
    {
        internal static bool IsEquivalentTo<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            return first.Except(second).Empty()
                && second.Except(first).Empty();
        }

        internal static bool Empty<T>(this IEnumerable<T> source)
        {
            return !source.Any();
        }
    }
}
