using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Idioms
{
    public class CompositeBehaviorExpectation : IBehaviorExpectation
    {
        private readonly IEnumerable<IBehaviorExpectation> expectations;

        public CompositeBehaviorExpectation(params IBehaviorExpectation[] behaviorExpectations)
        {
            this.expectations = behaviorExpectations;
        }

        public CompositeBehaviorExpectation(IEnumerable<IBehaviorExpectation> behaviorExpectations)
            : this(behaviorExpectations.ToArray())
        {
        }

        #region IBehaviorExpectation Members

        public void Verify(IGuardClauseCommand command)
        {
            foreach (var be in this.BehaviorExpectations)
            {
                be.Verify(command);
            }
        }

        #endregion

        public IEnumerable<IBehaviorExpectation> BehaviorExpectations
        {
            get { return this.expectations; }
        }
    }
}
