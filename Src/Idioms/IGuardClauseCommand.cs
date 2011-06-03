using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Idioms
{
    public interface IGuardClauseCommand
    {
        MemberInfo MemberInfo { get; }

        Type ContextType { get; }

        void Execute(object value);

        Exception Throw(string value);

        void Throw(string value, Exception innerException);
    }
}
