using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Ploeh.AutoFixture;

namespace Ploeh.AutoFixtureUnitTest.Dsl
{
    public class DelegatingSpecimenBuilderComposer : ISpecimenBuilderComposer
    {
        public DelegatingSpecimenBuilderComposer()
        {
            this.OnCompose = () => new DelegatingSpecimenBuilder();
        }

        #region ISpecimenBuilderComposer Members

        public ISpecimenBuilder Compose()
        {
            return this.OnCompose();
        }

        #endregion

        internal Func<ISpecimenBuilder> OnCompose { get; set; }
    }
}
