using System;

namespace Ploeh.AutoFixture.Idioms
{
    public interface IBoundaryBehavior
    {
        void Exercise(Action<object> action);

        bool IsSatisfiedBy(Type exceptionType);

        string Description { get; }
    }
}