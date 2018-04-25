using System;
using AutoFixture.Idioms;

namespace AutoFixture.IdiomsUnitTest
{
    public class DelegatingGuardClauseCommand : IGuardClauseCommand
    {
        public DelegatingGuardClauseCommand()
        {
            this.RequestedType = typeof(object);
            this.OnExecute = v => { };
            this.OnCreateException = v => new Exception();
            this.OnCreateExceptionWithInner = (v, e) => new Exception();
            this.OnCreateExceptionWithFailureReason = (v, r, e) => new Exception();
        }

        public Action<object> OnExecute { get; set; }

        public Func<string, Exception> OnCreateException { get; set; }

        public Func<string, Exception, Exception> OnCreateExceptionWithInner { get; set; }

        public Func<string, string, Exception, Exception> OnCreateExceptionWithFailureReason { get; set; }

        public Type RequestedType { get; set; }

        public string RequestedParameterName { get; set; }

        public void Execute(object value)
        {
            this.OnExecute(value);
        }

        public Exception CreateException(string value)
        {
            return this.OnCreateException(value);
        }

        public Exception CreateException(string value, Exception innerException)
        {
            return this.OnCreateExceptionWithInner(value, innerException);
        }

        public Exception CreateException(string value, string customError, Exception innerException)
        {
            return this.OnCreateExceptionWithFailureReason(value, customError, innerException);
        }
    }
}
