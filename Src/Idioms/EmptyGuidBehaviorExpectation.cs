using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Idioms
{
    public class EmptyGuidBehaviorExpectation : IBehaviorExpectation
    {
        #region IBehaviorExpectation Members

        public void Verify(IGuardClauseCommand command)
        {
            if (command.RequestedType != typeof(Guid))
                return;

            try
            {
                command.Execute(Guid.Empty);
            }
            catch (ArgumentException)
            {
                return;
            }
            catch (Exception e)
            {
                throw command.CreateException("\"Guid.Empty\"", e);
            }

            throw command.CreateException("\"Guid.Empty\"");
        }

        #endregion
    }
}
