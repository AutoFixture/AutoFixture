using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Idioms
{
    public class NullReferenceBehaviorExpectation : IBehaviorExpectation
    {
        #region IBehaviorExpectation Members

        public void Verify(IContextualCommand command)
        {
            if (!command.ContextType.IsClass
                && !command.ContextType.IsInterface)
            {
                return;
            }
            command.Execute(null);
        }

        #endregion
    }
}
