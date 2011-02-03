using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    internal static class LightweightMaybe
    {
        internal static IEnumerable<T> Maybe<T>(this T value)
        {
            return new[] { value };
        }
    }
}
