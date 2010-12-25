using System;
using System.Globalization;
using System.Reflection;

namespace Ploeh.AutoFixture.Idioms
{
    public static class BoundaryBehavior
    {
        public static void Assert(this ExceptionBoundaryBehavior behavior, Action<object> action)
        {
            if (behavior == null)
            {
                throw new ArgumentNullException("behavior");
            }
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            try
            {
                behavior.Exercise(action);
            }
            catch (Exception e)
            {
                if (behavior.IsSatisfiedBy(e))
                {
                    return;
                }
            }

            throw new BoundaryConventionException(
                string.Format(CultureInfo.CurrentCulture,
                     "The action did not throw the expected exception for the value {0}.", behavior.Description));
        }
    }
}