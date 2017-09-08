using System;
using Ploeh.AutoFixture.Idioms;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class DelegatingGuardClauseCommand : IGuardClauseCommand
    {
        public DelegatingGuardClauseCommand()
        {
            this.RequestedType = typeof(object);
            this.OnExecute = v => { };
            this.OnCreateException = v => new Exception();
            this.OnCreateExceptionWithInner = (v, e) => new Exception();
        }

        public Action<object> OnExecute { get; set; }

        public Func<string, Exception> OnCreateException { get; set; }

        public Func<string, Exception, Exception> OnCreateExceptionWithInner { get; set; }

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
    }
}
