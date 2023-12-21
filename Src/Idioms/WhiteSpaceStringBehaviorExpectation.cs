using System;
using System.Globalization;

namespace AutoFixture.Idioms
{
    /// <summary>
    /// Encapsulates the expected behavior when an <see cref="IGuardClauseCommand" /> (typically
    /// representing a method or constructor) is invoked with a white space argument.
    /// </summary>
    /// <seealso cref="Verify(IGuardClauseCommand)" />
    public class WhiteSpaceStringBehaviorExpectation : IBehaviorExpectation
    {
        /// <summary>
        /// Verifies the behavior of the command when invoked with white space.
        /// </summary>
        /// <param name="command">The command whose behavior must be examined.</param>
        /// <remarks>
        /// <para>
        /// This method encapsulates the behavior which is expected when a method or constructor is
        /// invoked with white space as one of the method arguments. In that case it's
        /// expected that invoking <paramref name="command" /> with white space throws an
        /// <see cref="ArgumentException" />, causing the Verify method to succeed. If other
        /// exceptions are thrown, or no exception is thrown when invoking the command, the Verify
        /// method throws an exception.
        /// </para>
        /// </remarks>
        public void Verify(IGuardClauseCommand command)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (command.RequestedType != typeof(string))
            {
                return;
            }

            try
            {
                command.Execute(" ");
            }
            catch (ArgumentException e)
            {
                if (string.Equals(e.ParamName, command.RequestedParameterName, StringComparison.Ordinal))
                {
                    return;
                }

                throw command.CreateInvalidParamNameException(WhiteSpace, e);
            }
            catch (Exception e)
            {
                throw command.CreateException(WhiteSpace, e);
            }

            throw command.CreateException(WhiteSpace);
        }

        private const string WhiteSpace = "<white space>";
    }
}
