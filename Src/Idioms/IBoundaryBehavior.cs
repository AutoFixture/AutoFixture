using System;

namespace Ploeh.AutoFixture.Idioms
{
    public interface IBoundaryBehavior
    {
        void Exercise(Action<object> action);

        bool IsSatisfiedBy(Exception exception);

        string Description { get; }
    }
}