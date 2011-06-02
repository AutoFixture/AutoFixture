using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Idioms
{
    public class GuardClauseAssertion : IdiomaticAssertion
    {
        private IBehaviorExpectation behaviorExpectation;

        public GuardClauseAssertion()
        {
        }

        public GuardClauseAssertion(IBehaviorExpectation behaviorExpectation)
        {
            this.behaviorExpectation = behaviorExpectation;
        }

        public IBehaviorExpectation BehaviorExpectation
        {
            get { return this.behaviorExpectation; }
        }
    }
}
