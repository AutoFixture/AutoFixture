using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Idioms
{
    public class ReflectionExceptionBoundaryBehavior : ExceptionBoundaryBehavior
    {
        private readonly ExceptionBoundaryBehavior behavior;

        public ReflectionExceptionBoundaryBehavior(ExceptionBoundaryBehavior behavior)
        {
            if (behavior == null)
            {
                throw new ArgumentNullException("behavior");
            }

            this.behavior = behavior;
        }

        public override void Exercise(Action<object> action)
        {
            this.behavior.Exercise(action);
        }

        public override bool IsSatisfiedBy(Exception exception)
        {
            var reflectionException = exception as TargetInvocationException;
            if (reflectionException != null)
            {
                return this.behavior.IsSatisfiedBy(reflectionException.InnerException);
            }

            return false;
        }

        public override string Description
        {
            get { return this.behavior.Description; }
        }
    }
}
