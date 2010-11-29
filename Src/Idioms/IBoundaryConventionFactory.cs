using System;

namespace Ploeh.AutoFixture.Idioms
{
    public interface IBoundaryConventionFactory
    {
        IBoundaryConvention GetConvention(Type type);
    }
}