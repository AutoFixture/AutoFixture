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
        private readonly ParameterInfo targetParameter;
        private readonly IDictionary<ParameterInfo, object> defaultArguments;

        public MethodInvokeCommand(IMethod method, ParameterInfo targetParameter, IDictionary<ParameterInfo, object> defaultArguments)
        {
            this.method = method;
            this.targetParameter = targetParameter;
            this.defaultArguments = defaultArguments;
        }

        public IMethod Method
        {
            get { return this.method; }
        }

        public IDictionary<ParameterInfo, object> DefaultArguments
        {
            get { return this.defaultArguments; }
        }

        public ParameterInfo TargetParameter
        {
            get { return this.targetParameter; }
        }

        #region IGuardClauseCommand Members

        public Type ValueType
        {
            get { throw new NotImplementedException(); }
        }

        public void Execute(object value)
        {
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
