using System;

namespace Ploeh.AutoFixture.Idioms
{
    public interface ITypeGuardSpecification
    {
        IBoundaryConvention IsSatisfiedBy(Type type);
    }
}