using System.Collections.Generic;
using System;

namespace Ploeh.AutoFixture.Idioms
{
    public interface IBoundaryConvention
    {
        IEnumerable<IBoundaryBehavior> CreateBoundaryBehaviors(Type type);
    }
}