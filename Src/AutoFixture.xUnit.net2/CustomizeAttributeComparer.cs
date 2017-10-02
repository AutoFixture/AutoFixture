using System.Collections.Generic;

namespace Ploeh.AutoFixture.Xunit2
{
    internal class CustomizeAttributeComparer : Comparer<IParameterCustomizationSource>
    {
        public override int Compare(IParameterCustomizationSource x, IParameterCustomizationSource y)
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