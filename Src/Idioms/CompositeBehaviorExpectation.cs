using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Idioms
{
    public class CompositeBehaviorExpectation : IBehaviorExpectation
    {
        #region IBehaviorExpectation Members

        public void Verify(IGuardClauseCommand command)
        {
        }

        #endregion
    }
}
