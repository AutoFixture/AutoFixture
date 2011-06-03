using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Idioms;
using System.Reflection;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class DelegatingContextualCommand : IContextualCommand
    {
        public DelegatingContextualCommand()
        {
            this.ContextType = typeof(object);
            this.OnExecute = v => { };
        }

        public Action<object> OnExecute { get; set; }

        #region IContextualCommand Members

        public MemberInfo MemberInfo { get; set; }

        public Type ContextType { get; set; }

        public void Execute(object value)
        {
            this.OnExecute(value);
        }

        #endregion
    }
}
