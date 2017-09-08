using System;
using Ploeh.AutoFixture.Idioms;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class DelegatingBehaviorExpectation : IBehaviorExpectation
    {
        public DelegatingBehaviorExpectation()
        {
            this.OnVerify = c => { };
        }

        public Action<IGuardClauseCommand> OnVerify { get; set; }

        public void Verify(IGuardClauseCommand command)
        {
            this.OnVerify(command);
        }
    }
}
