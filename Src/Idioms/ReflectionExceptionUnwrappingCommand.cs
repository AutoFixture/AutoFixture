using System;
using System.Reflection;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Decorates another <see cref="IGuardClauseCommand" /> and unwraps
    /// <see cref="TargetInvocationException" /> occurances from the <see cref="Execute" /> method.
    /// </summary>
    public class ReflectionExceptionUnwrappingCommand : IGuardClauseCommand
    {
        private readonly IGuardClauseCommand command;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReflectionExceptionUnwrappingCommand" />
        /// class.
        /// </summary>
        /// <param name="command">The decorated command.</param>
        public ReflectionExceptionUnwrappingCommand(IGuardClauseCommand command)
        {
            this.command = command;
        }

        /// <summary>
        /// Gets the decorated command supplied via the constructor.
        /// </summary>
        public IGuardClauseCommand Command
        {
            get { return this.command; }
        }

        /// <summary>
        /// Gets the type of the requested value.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The return value is the return value of the decorated <see cref="Command" /> instance's
        /// <see cref="IGuardClauseCommand.RequestedType" /> property.
        /// </para>
        /// </remarks>
        public Type RequestedType
        {
            get { return this.command.RequestedType; }
        }

        /// <summary>
        /// Gets the parameter name of the requested value.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The return value is the return value of the decorated <see cref="Command" /> instance's
        /// <see cref="IGuardClauseCommand.RequestedParameterName" /> property.
        /// </para>
        /// </remarks>
        public string RequestedParameterName
        {
            get { return this.command.RequestedParameterName; }
        }

        /// <summary>
        /// Executes the action on the decorated <see cref="Command" />. If a
        /// <see cref="TargetInvocationException" /> is thrown, it's unwrapped and its
        /// <see cref="Exception.InnerException" /> is thrown instead.
        /// </summary>
        /// <param name="value">The value with wich the action is executed.</param>
        public void Execute(object value)
        {
            try
            {
                this.Command.Execute(value);
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }
        }

        /// <summary>
        /// Creates an exception which communicates that an error occured for a specific input
        /// value.
        /// </summary>
        /// <param name="value">A string representation of the value.</param>
        /// <returns>
        /// The exception created by the decorated <see cref="Command" />.
        /// </returns>
        public Exception CreateException(string value)
        {
            return this.Command.CreateException(value);
        }

        /// <summary>
        /// Creates an exception which communicates that an error occured for a specific input
        /// value.
        /// </summary>
        /// <param name="value">A string representation of the value.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        /// <returns>
        /// The exception created by the decorated <see cref="Command" />.
        /// </returns>
        public Exception CreateException(string value, Exception innerException)
        {
            return this.Command.CreateException(value, innerException);
        }
    }
}
