using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.TestTypeFoundation
{
    public class StaticPropertyHolder<T>
    {
        public static T Property { get; set; }
    }
}
