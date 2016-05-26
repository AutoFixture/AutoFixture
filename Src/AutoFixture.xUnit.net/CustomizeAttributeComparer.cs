﻿using System.Collections.Generic;

namespace Ploeh.AutoFixture.Xunit
{
    internal class CustomizeAttributeComparer : Comparer<AutoFixture.CustomizeAttribute>
    {
        public override int Compare(AutoFixture.CustomizeAttribute x, AutoFixture.CustomizeAttribute y)
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