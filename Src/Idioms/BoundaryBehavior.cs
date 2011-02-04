using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Idioms
{
    internal static class BoundaryBehavior
    {
        internal static IBoundaryBehavior UnwrapReflectionExceptions(this IBoundaryBehavior behavior)
        {
            var exceptionBehavior = behavior as ExceptionBoundaryBehavior;
            if (exceptionBehavior == null)
            {
                return behavior;
            }

            return new ReflectionExceptionBoundaryBehavior(exceptionBehavior);
        }
    }
}
