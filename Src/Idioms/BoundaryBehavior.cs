using System;
using System.Globalization;
using System.Reflection;

namespace Ploeh.AutoFixture.Idioms
{
    public static class BoundaryBehavior
    {
        public static void Assert(this IBoundaryBehavior behavior, Action<object> action)
        {
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

            throw new ValueGuardConventionException(
                string.Format(CultureInfo.CurrentCulture,
                     "The action did not throw the expected exception for the value {0}.", behavior.Description));
        }
    }
}