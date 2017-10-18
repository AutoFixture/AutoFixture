namespace AutoFixture.Kernel
{
    /// <summary>
    /// Represents a command which can be applied to a specimen.
    /// </summary>
    public interface ISpecimenCommand
    {
        /// <summary>Applies the command to the supplied specimen.</summary>
        /// <param name="specimen">
        /// A specimen upon which the command should be applied.
        /// </param>
        /// <param name="context">
        /// A context that can be used to create other specimens.
        /// </param>
        /// <remarks>
        /// <para>
        /// Implementations of this interface will often somehow mutate
        /// <paramref name="specimen" />, e.g. by assigning values to
        /// properties, alternatively pulling those values from
        /// <paramref name="context" />. Another option is that the the
        /// implementation changes the state of command itself. No matter the
        /// details, since this is a Command, it can be expected to mutate
        /// state somewhere.
        /// </para>
        /// </remarks>
        void Execute(object specimen, ISpecimenContext context);
    }
}
