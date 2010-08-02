using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Xunit
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class FrozenAttribute : Attribute
    {
    }
}
