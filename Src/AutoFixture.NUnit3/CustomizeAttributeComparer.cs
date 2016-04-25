using System.Collections.Generic;

namespace Ploeh.AutoFixture.NUnit3
{
    internal class CustomizeAttributeComparer : Comparer<CustomizeAttribute>
    {
        public override int Compare(CustomizeAttribute x, CustomizeAttribute y)
        {
            var xfrozen = x is FrozenAttribute;
            var yfrozen = y is FrozenAttribute;

            if (xfrozen && !yfrozen)
            {
                return 1;
            }

            if (yfrozen && !xfrozen)
            {
                return -1;
            }

            return 0;
        }
    }
}