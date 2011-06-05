using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Idioms
{
    public class ConstructorInvokeCommand : IGuardClauseCommand
    {
        private readonly ConstructorInfo constructorInfo;
        private readonly ParameterInfo targetParameter;
        private readonly IDictionary<ParameterInfo, object> defaultArguments;

        public ConstructorInvokeCommand(ConstructorInfo constructorInfo, ParameterInfo targetParameter, IDictionary<ParameterInfo, object> defaultArguments)
        {
            this.constructorInfo = constructorInfo;
            this.targetParameter = targetParameter;
            this.defaultArguments = defaultArguments;
        }

        public ConstructorInfo ConstructorInfo
        {
            get { return this.constructorInfo; }
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
