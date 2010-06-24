using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    internal class DelegatingSpecifiedSpecimenCommand<T> : DelegatingRequestSpecification, ISpecifiedSpecimenCommand<T>
    {
        public DelegatingSpecifiedSpecimenCommand()
        {
            this.OnExecute = (s, c) => { };
        }

        #region ISpecifiedSpecimenCommand<T> Members

        public void Execute(T specimen, ISpecimenContext container)
        {
            this.OnExecute(specimen, container);
        }

        #endregion

        internal Action<T, ISpecimenContext> OnExecute { get; set; }
    }
}
