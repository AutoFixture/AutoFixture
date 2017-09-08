using System;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Encapsulates expectations about the behavior of a method or constructor when it's invoked
    /// with a null argument.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The NullReferenceBehaviorExpectation class encapsulates the following expectation: when an
    /// action (such as a method call or constructor invocation) is performed with a
    /// <see langword="null"/> argument it should raise an <see cref="ArgumentNullException" />. If
    /// this happens the expectation is verified. If no exception, or any other type of exception,
    /// is thrown the expectation isn't met.
    /// </para>
    /// </remarks>
    public class NullReferenceBehaviorExpectation : IBehaviorExpectation
    {
        /// <summary>
        /// Verifies that the command behaves correct when invoked with a null argument.
        /// </summary>
        /// <param name="command">The command whose behavior must be examined.</param>
        /// <remarks>
        /// <para>
        /// The Verify method attempts to invoke the <paramref name="command" /> instance's
        /// <see cref="IGuardClauseCommand.Execute" /> with <see langword="null" />. The expected
        /// result is that this action throws an <see cref="ArgumentNullException" /> with proper parameter name, 
        /// in which case the expected behavior is considered verified. If any other exception is thrown, or
        /// if no exception is thrown at all, the verification fails and an exception is thrown.
        /// </para>
        /// <para>
        /// The behavior is only asserted if the command's
        /// <see cref="IGuardClauseCommand.RequestedType" /> is nullable. In case of value types,
        /// no action is performed.
        /// </para>
        /// </remarks>
        public void Verify(IGuardClauseCommand command)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            if (!command.RequestedType.IsClass
                && !command.RequestedType.IsInterface)
            {
                return;
            }

            try
            {
                command.Execute(null);
            }
            catch (ArgumentNullException e)
            {
                if (e.ParamName == command.RequestedParameterName)
                    return;
                throw command.CreateException("null", e);
            }
            catch (Exception e)
            {
                throw command.CreateException("null", e);
            }

            throw command.CreateException("null");
        }
    }
}
