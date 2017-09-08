using System;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Represents an action which can be invoked to verify whether or not an expectation about
    /// Guard Clause behavior is met, and what to do if that is not the case.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This interface is a rather specialized interface supporting the implementation of
    /// <see cref="GuardClauseAssertion" />.
    /// </para>
    /// </remarks>
    public interface IGuardClauseCommand
    {
        /// <summary>
        /// Gets the type of the requested value.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The RequestedType property identifies the type of object which should be supplied to
        /// the <see cref="Execute" /> method.
        /// </para>
        /// </remarks>
        Type RequestedType { get; }

        /// <summary>
        /// Gets the name of the requested parameter.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The RequestedParameterName property identifies the name of parameter which should be supplied to
        /// the <see cref="Execute" /> method.
        /// </para>
        /// </remarks>
        string RequestedParameterName { get; }

        /// <summary>
        /// Executes the action with the specified value.
        /// </summary>
        /// <param name="value">The value with wich the action is executed.</param>
        /// <remarks>
        /// <para>
        /// The <paramref name="value" /> is expected to be compatible with
        /// <see cref="RequestedType" />. It should be possible to cast the value to the requested
        /// type.
        /// </para>
        /// </remarks>
        void Execute(object value);

        /// <summary>
        /// Creates an exception which communicates that an error occured for a specific input
        /// value.
        /// </summary>
        /// <param name="value">A string representation of the value.</param>
        /// <returns>An exception which communicates the cause of the error.</returns>
        /// <remarks>
        /// <para>
        /// The <paramref name="value" /> is a string representation of the value supplied to the
        /// <see cref="Execute" /> method. Together with the context contained within any
        /// implementation of <see cref="IGuardClauseCommand" /> this value can be used to build an
        /// exception message.
        /// </para>
        /// </remarks>
        Exception CreateException(string value);

        /// <summary>
        /// Creates an exception which communicates that an error occured for a specific input
        /// value.
        /// </summary>
        /// <param name="value">A string representation of the value.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        /// <returns>An exception which communicates the cause of the error.</returns>
        /// <remarks>
        /// <para>
        /// The <paramref name="value" /> is a string representation of the value supplied to the
        /// <see cref="Execute" /> method. Together with the context contained within any
        /// implementation of <see cref="IGuardClauseCommand" /> this value can be used to build an
        /// exception message.
        /// </para>
        /// </remarks>
        Exception CreateException(string value, Exception innerException);
    }
}