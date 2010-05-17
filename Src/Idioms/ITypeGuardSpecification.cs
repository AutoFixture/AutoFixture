using System;

namespace Ploeh.AutoFixture.Idioms
{
    public interface ITypeGuardSpecification
    {
        IValueGuardConvention IsSatisfiedBy(Type type);
    }
}