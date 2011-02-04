using System;
using System.Globalization;

namespace Ploeh.AutoFixture.Idioms
{
    public abstract class ExceptionBoundaryBehavior : IBoundaryBehavior
    {
        public void Assert(Action<object> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            try
            {
                this.Exercise(action);
            }
            catch (Exception e)
            {
                if (this.IsSatisfiedBy(e))
                {
                    return;
                }
            }

            throw new BoundaryConventionException(
                string.Format(CultureInfo.CurrentCulture,
                     "The action did not throw the expected exception for the value {0}.", this.Description));
        }

        public abstract void Exercise(Action<object> action);

        public abstract bool IsSatisfiedBy(Exception exception);

        public abstract string Description { get; }
    }
}