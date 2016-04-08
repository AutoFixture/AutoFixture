using System.Collections.Generic;

namespace Ploeh.AutoFixture.Xunit
{
    internal class CustomizeAttributeComparer : Comparer<CustomizeAttribute>
    {
        public override int Compare(CustomizeAttribute x, CustomizeAttribute y)
        {
            if (x is FrozenAttribute && !(y is FrozenAttribute))
            {
                return 1;
            }

            if (y is FrozenAttribute && !(x is FrozenAttribute))
            {
                return -1;
            }

            return 0;
        }
    }
}