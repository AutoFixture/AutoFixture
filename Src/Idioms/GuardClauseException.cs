using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Idioms
{
    public class GuardClauseException : Exception
    {
        private Exception e;

        public GuardClauseException(Exception e)
            : base(Guid.NewGuid().ToString(), e)
        {
            // TODO: Complete member initialization
            this.e = e;
        }

        public GuardClauseException()
        {
            // TODO: Complete member initialization
        }
    }
}
