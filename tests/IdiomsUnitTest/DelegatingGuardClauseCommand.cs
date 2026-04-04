using System;
using AutoFixture.Idioms;

namespace AutoFixture.IdiomsUnitTest;

public class DelegatingGuardClauseCommand : IGuardClauseCommand
{
    public DelegatingGuardClauseCommand()
    {
        RequestedType = typeof(object);
        OnExecute = v => { };
        OnCreateException = v => new Exception();
        OnCreateExceptionWithInner = (v, e) => new Exception();
        OnCreateExceptionWithFailureReason = (v, r, e) => new Exception();
    }

    public Action<object> OnExecute { get; set; }

    public Func<string, Exception> OnCreateException { get; set; }

    public Func<string, Exception, Exception> OnCreateExceptionWithInner { get; set; }

    public Func<string, string, Exception, Exception> OnCreateExceptionWithFailureReason { get; set; }

    public Type RequestedType { get; set; }

    public string RequestedParameterName { get; set; }

    public void Execute(object value)
    {
        OnExecute(value);
    }

    public Exception CreateException(string value)
    {
        return OnCreateException(value);
    }

    public Exception CreateException(string value, Exception innerException)
    {
        return OnCreateExceptionWithInner(value, innerException);
    }

    public Exception CreateException(string value, string customError, Exception innerException)
    {
        return OnCreateExceptionWithFailureReason(value, customError, innerException);
    }
}