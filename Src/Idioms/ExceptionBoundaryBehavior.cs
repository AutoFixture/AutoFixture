using System;
using System.Globalization;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Base class for verifying that a particular boundary (member) throws an expected exception
    /// when invoked with a particular parameter.
    /// </summary>
    public abstract class ExceptionBoundaryBehavior : IBoundaryBehavior
    {
        /// <summary>
        /// Asserts that the specified action throws an expected exception when invoked.
        /// </summary>
        /// <param name="action">The action to verify.</param>
        /// <param name="context">A text describing the context of <paramref name="action"/>.</param>
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

                throw new BoundaryConventionException(
                    string.Format(CultureInfo.CurrentCulture,
                        "{0} did not throw the expected exception for the value: {1}.", context, this.Description),
                    e);
            }

            throw new BoundaryConventionException(
                string.Format(CultureInfo.CurrentCulture,
                     "{0} did not throw the expected exception for the value: {1}.", context, this.Description));
        }

        /// <summary>
        /// Exercises the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        public abstract void Exercise(Action<object> action);

        /// <summary>
        /// Determines whether an exception satisfies the expectation of this behavior.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="exception"/> satisfies the expectation of
        /// this behavior; otherwise, <see langword="false"/>.
        /// </returns>
        public abstract bool IsSatisfiedBy(Exception exception);

        /// <summary>
        /// Gets the description of the provocation which is expected to throw an exception.
        /// </summary>
        public abstract string Description { get; }
    }
}