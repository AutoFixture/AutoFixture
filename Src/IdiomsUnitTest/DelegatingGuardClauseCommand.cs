using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Idioms;
using System.Reflection;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class DelegatingGuardClauseCommand : IGuardClauseCommand
    {
        public DelegatingGuardClauseCommand()
        {
            this.ValueType = typeof(object);
            this.OnExecute = v => { };
            this.OnCreateException = v => new Exception();
            this.OnCreateExceptionWithInner = (v, e) => new Exception();
        }

        public Action<object> OnExecute { get; set; }

        public Func<string, Exception> OnCreateException { get; set; }

        public Func<string, Exception, Exception> OnCreateExceptionWithInner { get; set; }

        #region IContextualCommand Members

        public Type ValueType { get; set; }

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

        #endregion
    }
}
