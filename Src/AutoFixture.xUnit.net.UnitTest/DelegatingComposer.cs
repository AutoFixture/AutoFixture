using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Xunit.UnitTest
{
    internal class DelegatingComposer : ICustomizableComposer
    {
        private readonly List<ISpecimenBuilder> customizations;
        private readonly List<ISpecimenBuilder> residueCollectors;

        public DelegatingComposer()
        {
            this.customizations = new List<ISpecimenBuilder>();
            this.residueCollectors = new List<ISpecimenBuilder>();
            this.OnCompose = () => new DelegatingSpecimenBuilder();
        }

        #region ICustomizableComposer Members

        public IList<ISpecimenBuilder> Customizations
        {
            get { return this.customizations; }
        }

        public IList<ISpecimenBuilder> ResidueCollectors
        {
            get { return this.residueCollectors; }
        }

        #endregion

        #region ISpecimenBuilderComposer Members

        public ISpecimenBuilder Compose()
        {
            return this.OnCompose();
        }

        #endregion

        internal Func<ISpecimenBuilder> OnCompose { get; set; }
    }
}
