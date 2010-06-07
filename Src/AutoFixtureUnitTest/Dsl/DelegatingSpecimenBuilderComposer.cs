using System;
using Ploeh.AutoFixture.Dsl;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;

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
