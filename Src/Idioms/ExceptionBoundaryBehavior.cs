using System;

namespace Ploeh.AutoFixture.Idioms
{
    public abstract class ExceptionBoundaryBehavior
    {
        public abstract void Exercise(Action<object> action);

        public abstract bool IsSatisfiedBy(Exception exception);

        public abstract string Description { get; }
    }
}