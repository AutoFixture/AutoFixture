using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    public class MapCreateManyToEnumerable : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            if (fixture == null)
                throw new ArgumentNullException("fixture");

            fixture.Customizations.Insert(0, new MultipleToEnumerableRelay());
        }
    }
}
