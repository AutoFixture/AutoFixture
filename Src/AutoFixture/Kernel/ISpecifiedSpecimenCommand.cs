using System;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// A Command that performs an action on a specimen and knows whether that action matches a
    /// given request.
    /// </summary>
    /// <typeparam name="T">The type of specimen on which the command acts.</typeparam>
    [Obsolete("Use ISpecimenCommand instead of Execute, and IRequestSpecification for the IsSatisfiedBy functionality.", true)]
    public interface ISpecifiedSpecimenCommand<T> : IRequestSpecification
    {
        /// <summary>
        /// Executes the command on the supplied specimen.
        /// </summary>
        /// <param name="specimen">The specimen on which the command is executed.</param>
        /// <param name="context">
        /// An <see cref="ISpecimenContext"/> that can be used to resolve other requests.
        /// </param>
        void Execute(T specimen, ISpecimenContext context);
    }
}
