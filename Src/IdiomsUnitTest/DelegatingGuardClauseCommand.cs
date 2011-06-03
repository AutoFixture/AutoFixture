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
            this.ContextType = typeof(object);
            this.MemberInfo = typeof(object).GetMembers().First();
            this.OnExecute = v => { };
            this.OnThrow = v => new Exception();
            this.OnThrowWithInner = (v, e) => new Exception();
        }

        public Action<object> OnExecute { get; set; }

        public Func<string, Exception> OnThrow { get; set; }

        public Func<string, Exception, Exception> OnThrowWithInner { get; set; }

        #region IContextualCommand Members

        public MemberInfo MemberInfo { get; set; }

        public Type ContextType { get; set; }

        public void Execute(object value)
        {
            this.OnExecute(value);
        }

        public Exception CreateException(string value)
        {
            return this.OnThrow(value);
        }

        public Exception CreateException(string value, Exception innerException)
        {
            return this.OnThrowWithInner(value, innerException);
        }

        #endregion
    }
}
