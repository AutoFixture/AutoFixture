using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Idioms
{
    public class CompositeBehaviorExpectation : IBehaviorExpectation
    {
        private readonly IBehaviorExpectation[] expectations;

        public CompositeBehaviorExpectation(params IBehaviorExpectation[] behaviorExpectations)
        {
            this.expectations = behaviorExpectations;
        }

        #region IBehaviorExpectation Members

        public void Verify(IGuardClauseCommand command)
        {
        }

        #endregion

        public IEnumerable<IBehaviorExpectation> BehaviorExpectations
        {
            get { return this.expectations; }
        }
    }
}
