using System.ComponentModel;

namespace Ploeh.TestTypeFoundation
{
    public class DataErrorInfo : IDataErrorInfo
    {
        public string Error => string.Empty;

        public string this[string columnName] => string.Empty;
    }
}
