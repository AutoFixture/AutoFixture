using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.TestTypeFoundation
{
    public class TypeWithGetOnlyProperty
    {
        public string GetOnlyProperty
        {
            get { return ""; }
        }
    }
}
