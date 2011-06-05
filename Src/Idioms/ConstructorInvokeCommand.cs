using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Idioms
{
    public class ConstructorInvokeCommand : IGuardClauseCommand
    {
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
