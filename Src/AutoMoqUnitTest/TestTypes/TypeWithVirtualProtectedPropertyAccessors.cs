using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.AutoMoq.UnitTest.TestTypes
{
    public class TypeWithVirtualProtectedPropertyAccessors
    {
        public virtual string PropertyWithProtectedGet { protected get; set; }
        public virtual string PropertyWithProtectedSet { get; protected set; }
    }
}
