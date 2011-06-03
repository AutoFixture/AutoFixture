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
            this.ContextType = typeof(object);
            this.OnExecute = () => { };
        }

        public Action OnExecute { get; set; }

        #region IContextualCommand Members

        public Type ContextType { get; set; }

        public void Execute()
        {
            this.OnExecute();
        }

        #endregion
    }
}
