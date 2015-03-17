using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest.TestTypes
{
    public interface IInterfaceWithNewMethod : IInterfaceWithShadowedMethod
    {
        //new method
        new string Method(int i);
    }
}
