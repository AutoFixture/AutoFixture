using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        #region IBehaviorExpectation Members

        public void Verify(IGuardClauseCommand command)
        {
            this.OnVerify(command);
        }

        #endregion
    }
}
