using System;
using System.Globalization;

namespace Ploeh.AutoFixture.Idioms
{
    public abstract class ExceptionBoundaryBehavior : IBoundaryBehavior
    {
        public void Assert(Action<object> action, string context)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            if (context == null)
            {
                throw new ArgumentNullException("context");
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
                throw;
            }

            throw new BoundaryConventionException(
                string.Format(CultureInfo.CurrentCulture,
                     "{0} did not throw the expected exception for the value: {1}.", context, this.Description));
        }

        public abstract void Exercise(Action<object> action);

        public abstract bool IsSatisfiedBy(Exception exception);

        public abstract string Description { get; }
    }
}