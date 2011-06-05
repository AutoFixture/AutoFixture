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

        public ConstructorInvokeCommand(ConstructorInfo constructorInfo)
        {
            this.constructorInfo = constructorInfo;
        }

        public ConstructorInfo ConstructorInfo
        {
            get { return this.constructorInfo; }
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
