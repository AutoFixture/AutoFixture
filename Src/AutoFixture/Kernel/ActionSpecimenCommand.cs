using System;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Adapts an action delegate to <see cref="ISpecimenCommand" />.
    /// </summary>
    /// <typeparam name="T">
    /// The type of specimen operated on by the adapted action delegate.
    /// </typeparam>
    public class ActionSpecimenCommand<T> : ISpecimenCommand
    {
        private readonly Action<T, ISpecimenContext> action;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ActionSpecimenCommand{T}" /> class.
        /// </summary>
        /// <param name="action">
        /// An action to be performed when <see cref="Execute" /> is invoked.
        /// </param>
        /// <seealso cref="ActionSpecimenCommand(Action{T, ISpecimenContext})" />
        public ActionSpecimenCommand(Action<T> action)
        {
            this.action = (s, c) => action(s);
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ActionSpecimenCommand{T}" /> class.
        /// </summary>
        /// <param name="action">
        /// An action to be performed when <see cref="Execute" /> is invoked.
        /// </param>
        /// <seealso cref="ActionSpecimenCommand(Action{T})" />
        public ActionSpecimenCommand(Action<T, ISpecimenContext> action)
        {
            this.action = action;
        }

        /// <summary>Invokes the adapted action on the specimen.</summary>
        /// <param name="specimen">
        /// The specimen on which to invoke the adapted action.
        /// </param>
        /// <param name="context">The context.</param>
        /// <remarks>
        /// <para>
        /// This method invokes the adapted action supplied via one of the
        /// constructors, passing <paramref name="specimen" /> and
        /// <paramref name="context" /> to the action.
        /// </para>
        /// </remarks>
        /// <seealso cref="ActionSpecimenCommand(Action{T})" />
        /// <seealso cref="ActionSpecimenCommand(Action{T, ISpecimenContext})" />
        public void Execute(object specimen, ISpecimenContext context)
        {
            this.action((T)specimen, context);
        }
    }
}
