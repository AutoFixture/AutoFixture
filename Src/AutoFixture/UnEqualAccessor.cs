using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture
{
    internal class UnEqualAccessor : Accessor
    {
        public UnEqualAccessor(UnexpectedInfo ui)
            : base(ui, null)
        {
        }

        internal override bool CanRead
        {
            get { return true; }
        }

        internal override bool CanWrite
        {
            get { return false; }
        }

        internal override void AssignOn(object obj)
        {
        }

        internal override object ReadFrom(object obj)
        {
            return new UnEqual();
        }
    }
}
