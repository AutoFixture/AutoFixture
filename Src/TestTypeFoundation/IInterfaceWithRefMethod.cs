using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.TestTypeFoundation
{
    public interface IInterfaceWithRefMethod
    {
        string Method(ref string s);
    }
}
