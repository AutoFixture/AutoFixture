using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Idioms
{
    public class ReflectionBoundaryBehavior : IBoundaryBehavior
    {
        private readonly IBoundaryBehavior behavior;

        public ReflectionBoundaryBehavior(IBoundaryBehavior behavior)
        {
            if (behavior == null)
            {
                throw new ArgumentNullException("behavior");
            }

            this.behavior = behavior;
        }

        #region IBoundaryBehavior Members

        public void Exercise(Action<object> action)
        {
            this.behavior.Exercise(action);
        }

        public bool IsSatisfiedBy(Exception exception)
        {
            var reflectionException = exception as TargetInvocationException;
            if (reflectionException != null)
            {
                return this.behavior.IsSatisfiedBy(reflectionException.InnerException);
            }

            return false;
        }

        public string Description
        {
            get { return this.behavior.Description; }
        }

        #endregion
    }
}
