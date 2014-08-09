using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.TestTypeFoundation
{
    public interface IInterfaceWithOutMethod
    {
        void Method(out int i);
    }
}
