using System.Collections.Generic;

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
