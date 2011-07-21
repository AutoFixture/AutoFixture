using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Idioms
{
    public class GuardClauseException : Exception
    {
        public GuardClauseException()
            : base("An invariant was not correctly protected. Are you missing a Guard Clause?")
        {
        }

        public GuardClauseException(string message)
            : base(message)
        {
        }

        public GuardClauseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
