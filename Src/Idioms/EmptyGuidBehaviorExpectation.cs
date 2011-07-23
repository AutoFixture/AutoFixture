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
        }

        #endregion
    }
}
