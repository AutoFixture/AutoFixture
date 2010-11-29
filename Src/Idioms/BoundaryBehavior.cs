using System;
using System.Globalization;
using System.Reflection;

namespace Ploeh.AutoFixture.Idioms
{
    public static class BoundaryBehavior
    {
        public static void ReflectionAssert(this IBoundaryBehavior behavior, Action<object> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            try
            {
                behavior.Exercise(action);
                throw new ValueGuardConventionException(
                    string.Format(CultureInfo.CurrentCulture,
                         "The action did not throw the expected exception for the invalid value {0}", behavior.Description));
            }
            catch (Exception e)
            {
                if (e is TargetInvocationException && behavior.IsSatisfiedBy(e.InnerException))
                {
                    return;
                }

                throw new ValueGuardConventionException(
                    string.Format(CultureInfo.CurrentCulture,
                         "The action did not throw the expected exception for the invalid value {0}", behavior.Description),
                    e);
            } 
        }
    }
}