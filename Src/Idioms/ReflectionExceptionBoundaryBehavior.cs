using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Decorates <see cref="ExceptionBoundaryBehavior"/> in order to unwrap
    /// <see cref="TargetInvocationException"/>.
    /// </summary>
    /// <seealso cref="IsSatisfiedBy"/>
    public class ReflectionExceptionBoundaryBehavior : ExceptionBoundaryBehavior
    {
        private readonly ExceptionBoundaryBehavior behavior;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReflectionExceptionBoundaryBehavior"/>
        /// class.
        /// </summary>
        /// <param name="behavior">The behavior to decorate.</param>
        public ReflectionExceptionBoundaryBehavior(ExceptionBoundaryBehavior behavior)
        {
            if (behavior == null)
            {
                throw new ArgumentNullException("behavior");
            }

            this.behavior = behavior;
        }

        /// <summary>
        /// Exercises the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        public override void Exercise(Action<object> action)
        {
            this.behavior.Exercise(action);
        }

        /// <summary>
        /// Determines whether an exception satisfies the expectation of this behavior.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="exception"/> is a
        /// <see cref="TargetInvocationException"/> and its inner exception satisfies the
        /// expectation of the decorated behavior; otherwise, <see langword="false"/>.
        /// </returns>
        /// <seealso cref="ReflectionExceptionBoundaryBehavior(ExceptionBoundaryBehavior)"/>
        public override bool IsSatisfiedBy(Exception exception)
        {
            var reflectionException = exception as TargetInvocationException;
            if (reflectionException != null)
            {
                return this.behavior.IsSatisfiedBy(reflectionException.InnerException);
            }

            return false;
        }

        /// <summary>
        /// Gets the description of the provocation which is expected to throw an exception.
        /// </summary>
        /// <value></value>
        public override string Description
        {
            get { return this.behavior.Description; }
        }
    }
}
