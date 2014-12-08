using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.AutoMoq.UnitTest.TestTypes
{
    public interface IInterfaceWithVirtualPropertyWithCircularDependency
    {
        IInterfaceWithVirtualPropertyWithCircularDependency Property { get; set; }
    }
}
