using System;
using System.Collections.Generic;

namespace Ploeh.AutoFixture.Idioms
{
    public interface IBoundaryConventionFactory
    {
        IBoundaryConvention GetConvention(Type type);
    }
}