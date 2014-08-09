using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.TestTypeFoundation
{
    public class TypeWithVirtualMembers
    {
        public virtual string VirtualProperty { get; set; }

        public virtual string VirtualMethod()
        {
            return "Awesome string";
        }
    }
}
