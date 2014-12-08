using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.AutoMoq.UnitTest.TestTypes
{
    public interface IInterfaceWithShadowedProperty
    {
        //shadowed property
        string Property { get; set; }
    }

    public interface IInterfaceWithNewProperty : IInterfaceWithShadowedProperty
    {
        //new property
        new string Property { get; set; }
    }
}
