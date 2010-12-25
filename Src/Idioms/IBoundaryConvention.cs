using System.Collections.Generic;
using System;

namespace Ploeh.AutoFixture.Idioms
{
    public interface IBoundaryConvention
    {
        IEnumerable<ExceptionBoundaryBehavior> CreateBoundaryBehaviors(Type type);
    }
}