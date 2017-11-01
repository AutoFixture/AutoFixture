using System;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Encapsulates an operation without identifying any property or field.
    /// </summary>
    /// <typeparam name="T">The type of specimen.</typeparam>
    [Obsolete("This class is no longer used, and will be removed in future versions.", true)]
    public class UnspecifiedSpecimenCommand<T> : ISpecifiedSpecimenCommand<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnspecifiedSpecimenCommand&lt;T&gt;"/>
        /// class.
        /// </summary>
        /// <param name="action">The action to perform on a specimen.</param>
        public UnspecifiedSpecimenCommand(Action<T> action)
        {
            this.Action = action ?? throw new ArgumentNullException(nameof(action));
        }

        /// <summary>
        /// Gets the action that can be performed on a specimen.
        /// </summary>
        public Action<T> Action { get; }

        /// <summary>
        /// Executes <see cref="Action"/> on the supplied specimen.
        /// </summary>
        /// <param name="specimen">The specimen on which the command is executed.</param>
        /// <param name="context">
        /// An <see cref="ISpecimenContext"/> that can be used to resolve other requests. Not
        /// used.
        /// </param>
        public void Execute(T specimen, ISpecimenContext context)
        {
            this.Action(specimen);
        }

        /// <summary>
        /// Evaluates a request for a specimen.
        /// </summary>
        /// <param name="request">The specimen request.</param>
        /// <returns>
        /// <see langword="false"/>.
        /// </returns>
        public bool IsSatisfiedBy(object request)
        {
            return false;
        }
    }
}
