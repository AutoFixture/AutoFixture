using System;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Encapsulates the expected behavior when an <see cref="IGuardClauseCommand" /> (typically
    /// representing a method or constructor) is invoked with a <see cref="Guid.Empty" /> argument.
    /// </summary>
    /// <seealso cref="Verify(IGuardClauseCommand)" />
    public class EmptyGuidBehaviorExpectation : IBehaviorExpectation
    {
        /// <summary>
        /// Verifies the behavior of the command when invoked with <see cref="Guid.Empty" />.
        /// </summary>
        /// <param name="command">The command whose behavior must be examined.</param>
        /// <remarks>
        /// <para>
        /// This method encapsulates the behavior which is expected when a method or constructor is
        /// invoked with <see cref="Guid.Empty" /> as one of the method arguments. In that case it's
        /// expected that invoking <paramref name="command" /> with Guid.Empty throws an
        /// <see cref="ArgumentException" />, causing the Verify method to succeed. If other
        /// exceptions are thrown, or no exception is thrown when invoking the command, the Verify
        /// method throws an exception.
        /// </para>
        /// </remarks>
        public void Verify(IGuardClauseCommand command)
        {
            if (command == null)
                throw new ArgumentNullException("command");
            
            if (command.RequestedType != typeof(Guid))
                return;

            try
            {
                command.Execute(Guid.Empty);
            }
            catch (ArgumentException)
            {
                return;
            }
            catch (Exception e)
            {
                throw command.CreateException("\"Guid.Empty\"", e);
            }

            throw command.CreateException("\"Guid.Empty\"");
        }
    }
}
