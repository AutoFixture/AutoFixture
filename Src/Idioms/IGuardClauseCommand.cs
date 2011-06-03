using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Idioms
{
    public interface IGuardClauseCommand
    {
        Type ContextType { get; }

        void Execute(object value);

        Exception CreateException(string value);

        Exception CreateException(string value, Exception innerException);
    }
}
