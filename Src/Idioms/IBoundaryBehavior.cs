using System;

namespace Ploeh.AutoFixture.Idioms
{
    public interface IBoundaryBehavior
    {
        void Assert(Action<object> action);

        bool IsSatisfiedBy(Type exceptionType);

        string Description { get; }
    }
}