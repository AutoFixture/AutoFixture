using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Idioms
{
    public class GuardClauseAssertion : IdiomaticAssertion
    {
        private readonly ISpecimenBuilderComposer composer;
        private readonly IBehaviorExpectation behaviorExpectation;

        public GuardClauseAssertion(ISpecimenBuilderComposer composer)
        {
            this.composer = composer;
        }

        public GuardClauseAssertion(ISpecimenBuilderComposer composer, IBehaviorExpectation behaviorExpectation)
        {
            this.composer = composer;
            this.behaviorExpectation = behaviorExpectation;
        }

        public ISpecimenBuilderComposer Composer
        {
            get { return this.composer; }
        }

        public IBehaviorExpectation BehaviorExpectation
        {
            get { return this.behaviorExpectation; }
        }
    }
}
