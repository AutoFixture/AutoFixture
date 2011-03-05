using System;
using System.Globalization;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Defines an expectation about the behavior of a given boundary (member) when invoked with an
    /// empty <see cref="Guid"/>.
    /// </summary>
    public class GuidBoundaryBehavior : ExceptionBoundaryBehavior
    {
        /// <summary>
        /// Exercises the specified action with <see cref="Guid.Empty"/>.
        /// </summary>
        /// <param name="action">The action.</param>
        public override void Exercise(Action<object> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            action(Guid.Empty);
        }

        /// <summary>
        /// Determines whether an exception satisfies the expectation of this behavior.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="exception"/> is an
        /// <see cref="ArgumentException"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public override bool IsSatisfiedBy(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }

            return exception is ArgumentException;
        }

        /// <summary>
        /// Gets the description of the provocation which is expected to throw an exception.
        /// </summary>
        public override string Description
        {
            get { return "empty Guid"; }
        }
    }
}