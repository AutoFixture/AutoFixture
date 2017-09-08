namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Represents an expectation about the behavior when an <see cref="IGuardClauseCommand" /> is
    /// invoked.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This interface is a rather specialized interface supporting the implementation of
    /// <see cref="GuardClauseAssertion" />.
    /// </para>
    /// </remarks>
    public interface IBehaviorExpectation
    {
        /// <summary>
        /// Verifies the behavior of the command.
        /// </summary>
        /// <param name="command">The command whose behavior must be examined.</param>
        void Verify(IGuardClauseCommand command);
    }
}
