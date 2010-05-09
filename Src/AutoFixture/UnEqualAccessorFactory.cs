using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture
{
    internal class UnEqualAccessorFactory : AccessorFactory
    {
        private readonly UnexpectedInfo ui;

        public UnEqualAccessorFactory(UnexpectedInfo ui)
        {
            this.ui = ui;
        }

        internal override Accessor CreateAccessor()
        {
            return new UnEqualAccessor(this.ui);
        }

        internal override Accessor CreateAssignment(Func<Type, string, object> valueCreator)
        {
            return new UnEqualAccessor(this.ui);
        }
    }
}
