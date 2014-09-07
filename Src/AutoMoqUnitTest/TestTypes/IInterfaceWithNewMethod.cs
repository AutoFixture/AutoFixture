using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.AutoMoq.UnitTest.TestTypes
{
    public interface IInterfaceWithShadowedMethod
    {
        //shadowed method
        string Method(int i);

        //overloads
        string Method();
        string Method(out int i);
        string Method(int i, int i2);
        string Method(string s);
    }

    public interface IInterfaceWithNewMethod : IInterfaceWithShadowedMethod
    {
        //new method
        new string Method(int i);
    }
}
