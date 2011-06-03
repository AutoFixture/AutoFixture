using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Idioms
{
    public class NullReferenceBehaviorExpectation : IBehaviorExpectation
    {
        #region IBehaviorExpectation Members

        public void Verify(IGuardClauseCommand command)
        {
            if (!command.ContextType.IsClass
                && !command.ContextType.IsInterface)
            {
                return;
            }
            try
            {
                command.Execute(null);
            }
            catch (ArgumentNullException)
            {
                return;
            }
            catch (Exception e)
            {
                throw new GuardClauseException(e);
            }
            throw new GuardClauseException();
        }

        #endregion
    }
}
