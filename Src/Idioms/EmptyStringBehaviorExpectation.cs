using System;

namespace AutoFixture.Idioms
{
    /// <summary>
    /// Encapsulates the expected behavior when an <see cref="IGuardClauseCommand" /> (typically
    /// representing a method or constructor) is invoked with a <see cref="string.Empty" /> argument.
    /// </summary>
    /// <seealso cref="Verify(IGuardClauseCommand)" />
    public class EmptyStringBehaviorExpectation : IBehaviorExpectation
    {
        /// <summary>
        /// Verifies that the command behaves correct when invoked with an empty string argument.
        /// </summary>
        /// <param name="command">The command whose behavior must be examined.</param>
        /// <remarks>
        /// <para>
        /// This method encapsulates the behavior which is expected when a method or constructor is
        /// invoked with <see cref="string.Empty" /> as one of the method arguments. In that case it's
        /// expected that invoking <paramref name="command" /> with string.Empty throws an
        /// <see cref="ArgumentException" />, causing the Verify method to succeed. If other
        /// exceptions are thrown, or no exception is thrown when invoking the command, the Verify
        /// method throws an exception.
        /// </para>
        /// </remarks>
        public void Verify(IGuardClauseCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            if (command.RequestedType != typeof(string))
                return;

            try
            {
                command.Execute(string.Empty);
            }
            catch (ArgumentException e)
            {
                if (string.Equals(e.ParamName, command.RequestedParameterName, StringComparison.Ordinal))
                    return;
                throw command.CreateException("<empty string>", e);
            }
            catch (Exception e)
            {
                throw command.CreateException("<empty string>", e);
            }

            throw command.CreateException("<empty string>");
        }
    }
}