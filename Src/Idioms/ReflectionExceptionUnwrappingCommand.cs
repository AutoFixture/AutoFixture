using System;
using System.Reflection;

namespace AutoFixture.Idioms;

/// <summary>
/// Decorates another <see cref="IGuardClauseCommand" /> and unwraps
/// <see cref="TargetInvocationException" /> occurances from the <see cref="Execute" /> method.
/// </summary>
public class ReflectionExceptionUnwrappingCommand : IGuardClauseCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ReflectionExceptionUnwrappingCommand" />
    /// class.
    /// </summary>
    /// <param name="command">The decorated command.</param>
    public ReflectionExceptionUnwrappingCommand(IGuardClauseCommand command)
    {
        Command = command;
    }

    /// <summary>
    /// Gets the decorated command supplied via the constructor.
    /// </summary>
    public IGuardClauseCommand Command { get; }

    /// <inheritdoc />
    public Type RequestedType => Command.RequestedType;

    /// <inheritdoc />
    public string RequestedParameterName => Command.RequestedParameterName;

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
            Command.Execute(value);
        }
        catch (TargetInvocationException e)
        {
            throw e.InnerException;
        }
    }

    /// <inheritdoc />
    public Exception CreateException(string value)
    {
        return Command.CreateException(value);
    }

    /// <inheritdoc />
    public Exception CreateException(string value, Exception innerException)
    {
        return Command.CreateException(value, innerException);
    }

    /// <inheritdoc />
    public Exception CreateException(string value, string customError, Exception innerException)
    {
        return Command.CreateException(value, customError, innerException);
    }
}