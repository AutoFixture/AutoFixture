using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Idioms;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class DelegatingContextualCommand : IContextualCommand
    {
        public DelegatingContextualCommand()
        {
            this.OnExecute = () => { };
        }

        public Action OnExecute { get; set; }

        #region IContextualCommand Members

        public void Execute()
        {
            this.OnExecute();
        }

        #endregion
    }
}
