using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Ploeh.TestTypeFoundation
{
    public class DataErrorInfo : IDataErrorInfo
    {
        #region IDataErrorInfo Members

        public string Error
        {
            get { return string.Empty; }
        }

        public string this[string columnName]
        {
            get { return string.Empty; }
        }

        #endregion
    }
}
