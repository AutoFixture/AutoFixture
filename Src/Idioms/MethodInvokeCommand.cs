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
        private readonly ParameterInfo parameterInfo;

        public MethodInvokeCommand(IMethod method, IExpansion expansion, ParameterInfo parameterInfo)
        {
            this.method = method;
            this.expansion = expansion;
            this.parameterInfo = parameterInfo;
        }

        public IMethod Method
        {
            get { return this.method; }
        }

        public IExpansion Expansion
        {
            get { return this.expansion; }
        }

        public ParameterInfo ParameterInfo
        {
            get { return this.parameterInfo; }
        }

        #region IGuardClauseCommand Members

        public Type RequestedType
        {
            get { return this.ParameterInfo.ParameterType; }
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
