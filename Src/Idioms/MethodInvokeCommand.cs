using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Idioms
{
    public class MethodInvokeCommand : IGuardClauseCommand
    {
        private readonly IMethod method;
        private readonly IExpansion expansion;

        public MethodInvokeCommand(IMethod method, IExpansion expansion)
        {
            this.method = method;
            this.expansion = expansion;
        }

        public IMethod Method
        {
            get { return this.method; }
        }

        public IExpansion Expansion
        {
            get { return this.expansion; }
        }

        #region IGuardClauseCommand Members

        public Type RequestedType
        {
            get { throw new NotImplementedException(); }
        }

        public void Execute(object value)
        {
            this.method.Invoke(this.expansion.Expand(value));
        }

        public Exception CreateException(string value)
        {
            throw new NotImplementedException();
        }

        public Exception CreateException(string value, Exception innerException)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
